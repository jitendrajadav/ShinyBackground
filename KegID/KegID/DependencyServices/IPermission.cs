using System.Threading.Tasks;

namespace KegID.DependencyServices
{
    public interface IPermission
    {
        Task<bool> VerifyStoragePermissions();
    }
}
