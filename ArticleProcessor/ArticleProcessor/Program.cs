using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ArticleProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = GetFileNameToProcess();

            var content = String.Empty;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    content = sr.ReadToEnd();
                }
            }

            var newContent = ProcessImagesTags(content);

            var newFileName = Path.GetFileNameWithoutExtension(fileName) + "_prev" + Path.GetExtension(fileName);
            newFileName = Path.Combine(Path.GetDirectoryName(fileName), newFileName);
            using (FileStream fs = new FileStream(newFileName, FileMode.CreateNew, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(newContent);
                }
            }

            Console.WriteLine("Processed filed saved to: ");
            Console.WriteLine(newFileName);
        }

        private static string ProcessImagesTags(string content)
        {
            Regex photoRegex = new Regex(@"\<img class\=\""image\"" src\=\""images\/.+\.jpg\"" \/\>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex srcRegex = new Regex(@"(?<=src="")(.+?)(?= "" \/\>)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var photoMatches = photoRegex.Matches(content);
            foreach(Match photoMatch in photoMatches)
            {
                if (photoMatch.Success)
                {
                    // get image src
                    var srcMatch = srcRegex.Match(photoMatch.Value);
                    if (srcMatch.Success)
                    {
                        var wrappedLinkedPhoto = String.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", srcMatch.Value, photoMatch.Value);
                        content = content.Replace(photoMatch.Value, wrappedLinkedPhoto);
                    }
                }
            }

            return content;
        }

        private static string GetFileNameToProcess()
        {
            string fileName = String.Empty;

            while(String.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                Console.WriteLine("Enter file name with path: ");
                fileName = Console.ReadLine();
            }

            return fileName;
        }
    }
}
