using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KegID.Services
{
    public interface IGeolocationService
    {
        Task<Location> OnGetLastLocationAsync();
        Task<Location> OnGetCurrentLocationAsync();
    }
}
