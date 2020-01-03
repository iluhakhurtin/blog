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
            Console.WriteLine("Enter postressql server address: ");
            var server = Console.ReadLine();

            Console.WriteLine("Enter user: ");
            var user = Console.ReadLine();

            Console.WriteLine("Enter password: ");
            var password = Console.ReadLine();

            string connectionString = string.Format("Server={0};Port=5432;Database=Blog;User Id={1};Password={2};",
                server, user, password);

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
            //var data = File.ReadAllBytes(filePath);
            //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            //{
            //    string sql = @"UPDATE ""Files"" SET ""Data"" = :Data
            //                    WHERE ""Id"" = '86c001a8-3975-8073-f1e3-067bb081d215';
            //                ";

            //    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            //    {
            //        var dataParam = command.Parameters.AddWithValue("Data", data);

            //        connection.Open();
            //        command.ExecuteNonQuery();
            //    }
            //}
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

                            InsertImage(originalFileName, originalImageBytes, previewFileName, previewImageBytes, connectionString);
                        }
                    }
                }
            }
        }

        private static void InsertImage(string originalImageFileName, byte[] originalImageFileData,
            string previewImageFileName, byte[] previewImageFileData, string connectionString)
        {
            var mimeType = "image/jpeg";
            var originalFileId = InsertFile(originalImageFileName, originalImageFileData, mimeType, connectionString);
            var previewFileId = InsertFile(previewImageFileName, previewImageFileData, mimeType, connectionString);

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

                Console.WriteLine("New image id: " + imageId.ToString());
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
    }
}
