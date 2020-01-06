using System;
using System.IO;
using Npgsql;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace FileImport
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = GetConnectionString();

            Console.WriteLine("Do you want to upload a file directly (1) or process and upload photos(2)?");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.D1)
            {
                UploadFile(connectionString);
            }
            else if(keyInfo.Key == ConsoleKey.D2)
            {
                ProcessAndUploadPhotos(connectionString);
            }
        }

        private static void UploadFile(string connectionString)
        {
            var filePath = GetFilePath();
            var mimeType = GetMimeType(filePath);
            var data = File.ReadAllBytes(filePath);
            var fileName = Path.GetFileName(filePath);

            var fileId = InsertFile(fileName, data, mimeType, connectionString);
            Console.WriteLine("New file id is: {0}", fileId);

            Console.WriteLine("Do you want to insert an image as well? (y/n)");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Y)
            {
                var imageId = InsertImage(fileId, fileId, connectionString);
                Console.WriteLine("New image with id {0} has been added", imageId);
            }
        }

        private static void ProcessAndUploadPhotos(string connectionString)
        {
            var photoFileNames = GetPhotoFileNames();

            Console.WriteLine("Generating preview files.");

            foreach (var photoFileName in photoFileNames)
            {
                using (var originalImage = Image.FromFile(photoFileName))
                {
                    using (var previewImage = GetPreviewImage(originalImage))
                    {
                        var previewFileName = Path.GetFileNameWithoutExtension(photoFileName);
                        previewFileName = previewFileName + "_prev.jpg";
                        using (MemoryStream ms = new MemoryStream())
                        {
                            previewImage.Save(ms, ImageFormat.Jpeg);
                            ms.Flush();
                            var previewImageBytes = ms.ToArray();

                            var originalFileName = Path.GetFileName(photoFileName);
                            var originalImageBytes = File.ReadAllBytes(photoFileName);

                            InsertImageWithFiles(originalFileName, originalImageBytes, previewFileName, previewImageBytes, connectionString);
                        }
                    }
                }
            }
        }

        private static void InsertImageWithFiles(string originalImageFileName, byte[] originalImageFileData,
            string previewImageFileName, byte[] previewImageFileData, string connectionString)
        {
            var mimeType = "image/jpeg";
            var originalFileId = InsertFile(originalImageFileName, originalImageFileData, mimeType, connectionString);
            var previewFileId = InsertFile(previewImageFileName, previewImageFileData, mimeType, connectionString);

            var imageId = InsertImage(originalFileId, previewFileId, connectionString);
            Console.WriteLine("New image id: " + imageId.ToString());
        }

        private static Guid InsertImage(Guid originalFileId, Guid previewFileId, string connectionString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                var imageId = Guid.NewGuid();

                string sql = @"INSERT INTO ""Images""(
                                ""Id"", ""PreviewFileId"", ""OriginalFileId"")
	                            VALUES(:Id, :PreviewFileId, :OriginalFileId);
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Id", imageId);
                    command.Parameters.AddWithValue("PreviewFileId", previewFileId);
                    command.Parameters.AddWithValue("OriginalFileId", originalFileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return imageId;
            }
        }

        private static Guid InsertFile(string fileName, byte[] fileData, string mimeType, string connectionString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                var fileId = Guid.NewGuid();

                string sql = @"INSERT INTO ""Files""(
                                ""Id"", ""Name"", ""Extension"", ""Data"", ""MimeType"")
	                            VALUES(:Id, :Name, :Extension, :Data, :MimeType);
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Id", fileId);
                    command.Parameters.AddWithValue("Name", fileName);
                    command.Parameters.AddWithValue("Extension", Path.GetExtension(fileName));
                    command.Parameters.AddWithValue("Data", fileData);
                    command.Parameters.AddWithValue("MimeType", mimeType);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return fileId;
            }
        }

        private static Image GetPreviewImage(Image originalImage)
        {
            int maxWidth = 1500;
            int maxHeight = 1000;

            if(originalImage.Width <= maxWidth && originalImage.Height <= maxHeight)
            {
                return originalImage;
            }

            var ratioX = (double)maxWidth / originalImage.Width;
            var ratioY = (double)maxHeight / originalImage.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(originalImage.Width * ratio);
            var newHeight = (int)(originalImage.Height * ratio);

            var newImage = ResizeImage(originalImage, newWidth, newHeight);

            return newImage;
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private static IEnumerable<string> GetPhotoFileNames()
        {
            string directoryPath = String.Empty;

            while (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Enter path to the directory with photos: ");
                directoryPath = Console.ReadLine();
            }

            var photoFiles = Directory.EnumerateFiles(directoryPath).Where(fn => fn.EndsWith(".jpg") || fn.EndsWith(".JPG"));
            if (photoFiles.FirstOrDefault() == null)
            {
                Console.WriteLine("No photo files found...");
            }
            else
            {
                Console.WriteLine("The files to be processed:");
                foreach (var photoFile in photoFiles)
                {
                    Console.WriteLine(photoFile);
                }
            }

            return photoFiles;
        }

        private static string GetFilePath()
        {
            string filePath = String.Empty;

            while (!File.Exists(filePath))
            {
                Console.WriteLine("Enter path to the file to upload: ");
                filePath = Console.ReadLine();
            }

            return filePath;
        }

        private static string GetConnectionString()
        {
            Console.WriteLine("Use localhost settings (y/n)?");
            string server;
            string user;
            string password;
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Y)
            {
                server = "localhost";
                user = "postgres";
                password = "123456";
            }
            else
            {
                Console.WriteLine("Enter postressql server address: ");
                server = Console.ReadLine();

                Console.WriteLine("Enter user: ");
                user = Console.ReadLine();

                Console.WriteLine("Enter password: ");
                password = Console.ReadLine();
            }

            string connectionString = string.Format("Server={0};Port=5432;Database=Blog;User Id={1};Password={2};",
                server, user, password);

            return connectionString;
        }

        private static string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            switch (extension.ToLower())
            {
                case ".png":
                    return "image/png";
                case ".jpg":
                case ".jpeg":
                case ".jfif":
                case ".pjpeg":
                case ".pjp":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                default:
                    Console.WriteLine("Enter file mime type:");
                    var mimeType = Console.ReadLine();
                    return mimeType;
            }
        }
    }
}
