namespace KegID.DependencyServices
{
    public interface IFileUtil
    {
        string[] GetFiles(string extension);
        string ReadAllText(string path);
    }
}
