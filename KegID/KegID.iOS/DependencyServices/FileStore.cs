using KegID.DependencyServices;
using System;

namespace KegID.iOS.DependencyServices
{
    public class FileStore : IFileStore
    {
        public string GetFilePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        /// <summary>
        /// Writes the file to local storage.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileName">File name.</param>
        /// <param name="bytes">Bytes.</param>
        public string WriteFile(string fileName, byte[] bytes)
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string localPath = System.IO.Path.Combine(localFolder, fileName);
            System.IO.File.WriteAllBytes(localPath, bytes); // write to local storage

            return localPath;
        }

    }
}