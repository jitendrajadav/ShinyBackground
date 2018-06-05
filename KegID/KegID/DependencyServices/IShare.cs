using System.Threading.Tasks;

namespace KegID.DependencyServices
{
    public interface IShare
    {
        Task Show(string title, string message, string filePath);
    }
}
