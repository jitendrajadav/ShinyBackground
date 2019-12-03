using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IInitializeMetaData
    {
        Task LoadInitializeMetaData();
        void DeleteInitializeMetaData();
    }
}
