using System;
using System.IO;
using Npgsql;

namespace FileImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = GetFilePath();
            while (!File.Exists(filePath))
            {
                filePath = GetFilePath();
            }

            Console.WriteLine("Enter server address: ");
            var server = Console.ReadLine();

            Console.WriteLine("Enter user: ");
            var user = Console.ReadLine();

            Console.WriteLine("Enter password: ");
            var password = Console.ReadLine();

            string connectionString = string.Format("Server={0};Port=5432;Database=Blog;User Id={1};Password={2};",
                server, user, password);

            var data = File.ReadAllBytes(filePath);
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string sql = @"UPDATE ""Files"" SET ""Data"" = :Data
                                WHERE ""Id"" = '86c001a8-3975-8073-f1e3-067bb081d215';
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var dataParam = command.Parameters.AddWithValue("Data", data);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private static string GetFilePath()
        {
            Console.WriteLine("Enter path to the file: ");
            var filePath = Console.ReadLine();
            return filePath;
        }
    }
}
