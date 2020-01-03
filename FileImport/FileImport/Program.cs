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
            var photoFiles = GetPhotoFiles();

            foreach(var photoFile in photoFiles)
            {
                using (var originalImage = Image.FromFile(photoFile))
                {
                    using(var previewImage = GetPreviewImage(originalImage))
                    {
                        var previewFileName = Path.GetFileNameWithoutExtension(photoFile);
                        previewFileName = previewFileName + "_prev.jpg";
                        using(Stream s = new FileStream(previewFileName, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            previewImage.Save(s, ImageFormat.Jpeg);
                        }
                    }
                }
            }

            //var filePath = GetFilePath();
            //while (!File.Exists(filePath))
            //{
            //    filePath = GetFilePath();
            //}

            //Console.WriteLine("Enter server address: ");
            //var server = Console.ReadLine();

            //Console.WriteLine("Enter user: ");
            //var user = Console.ReadLine();

            //Console.WriteLine("Enter password: ");
            //var password = Console.ReadLine();

            //string connectionString = string.Format("Server={0};Port=5432;Database=Blog;User Id={1};Password={2};",
            //    server, user, password);

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

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

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

        private static IEnumerable<string> GetPhotoFiles()
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
            Console.WriteLine("Enter path to the file: ");
            var filePath = Console.ReadLine();
            return filePath;
        }
    }
}
