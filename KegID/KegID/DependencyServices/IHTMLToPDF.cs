using System.Threading.Tasks;

namespace KegID.DependencyServices
{
    public interface IHTMLToPDF
    {
        string SafeHTMLToPDF(string html, string filename);
    }
}
