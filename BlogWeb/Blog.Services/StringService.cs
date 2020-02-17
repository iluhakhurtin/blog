using System;
namespace Blog.Services
{
    public interface IStringService
    {
        String[] ParseCsvStringToArray(string csvString);
    }

    public class StringService : Service, IStringService
    {
        public StringService()
        {

        }

        public string[] ParseCsvStringToArray(string csvString)
        {
            char[] separators = new char[] { ',', ' ' };
            return csvString.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
