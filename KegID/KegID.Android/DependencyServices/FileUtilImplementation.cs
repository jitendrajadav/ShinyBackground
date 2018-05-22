using KegID.DependencyServices;
using KegID.Droid.DependencyServices;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(FileUtilImplementation))]
namespace KegID.Droid.DependencyServices
{
    public class FileUtilImplementation : IFileUtil
    {
        public string[] GetFiles(string extension)
        {
            string[] fileList = null;
            string path = "/storage/sdcard0/Download/";
            if (!Directory.Exists(path))
            {
                path = Android.OS.Environment.ExternalStorageDirectory + "/" + Android.OS.Environment.DirectoryDownloads + "/";
                if (!Directory.Exists(path))
                {
                    path = "";
                }
            }
            if (path.Length > 0)
            {
                fileList = Directory.GetFiles(path, "*." + extension);
            }
            return fileList;
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }

}