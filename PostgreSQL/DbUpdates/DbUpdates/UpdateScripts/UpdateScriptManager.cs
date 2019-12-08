using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace DbUpdates.UpdateScripts
{
    class UpdateScriptManager : IUpdateScriptManager
    {
        private string updatesFolderPath;

        public UpdateScriptManager(string updatesFolderPath)
        {
            this.updatesFolderPath = updatesFolderPath;
        }

        public IEnumerable<string> GetUpdatesContent(int version)
        {
            var updateFiles = this.GetUpdateFiles(version);
            foreach (var updateFileInfo in updateFiles)
            {
                yield return File.ReadAllText(updateFileInfo.FullName);
            }
        }

        private IEnumerable<FileInfo> GetUpdateFiles(int version)
        {
            string firstFileName = String.Concat(version.ToString("00000"), ".sql");

            DirectoryInfo directoryInfo = new DirectoryInfo(this.updatesFolderPath);
            var allFiles = directoryInfo.GetFiles();
            var actualUpdateFiles = allFiles.OrderBy(fi => fi.Name).SkipWhile(fi => fi.Name != firstFileName);
            return actualUpdateFiles;
        }
    }
}
