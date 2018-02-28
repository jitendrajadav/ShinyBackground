using System.Threading.Tasks;

namespace KegID.DependencyServices
{
    public interface IDialer
    {
        Task<bool> DialAsync(string number);
    }
}
