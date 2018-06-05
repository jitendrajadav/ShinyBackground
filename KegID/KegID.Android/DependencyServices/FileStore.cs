using System.IO;
using System.Threading.Tasks;
using KegID.DependencyServices;

namespace KegID.Droid.DependencyServices
{
    public class FileStore : IFileStore
    {
        public string GetFilePath()
        {
            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "image.png");
        }

        /// <summary>
        /// Writes the file to local storage.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileName">File name.</param>
        /// <param name="bytes">Bytes.</param>
        public string WriteFile(string fileName, byte[] bytes)
        {
            //var localFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;

            //string localPath = Path.Combine(localFolder, fileName);
            //File.WriteAllBytes(localPath, bytes); // write to local storage

            //return string.Format("file://{0}/{1}", localFolder, fileName);

            var directory = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            directory = Path.Combine(directory, Android.OS.Environment.DirectoryDownloads);
            string localFolder = Path.Combine(directory.ToString(), fileName);
            File.WriteAllBytes(localFolder, bytes);
            return string.Format("file://{0}/{1}", directory, fileName);
        }

    }
}