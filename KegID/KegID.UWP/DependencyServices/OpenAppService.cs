using System;
using System.Threading.Tasks;
using KegID.DependencyServices;
using KegID.UWP.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(OpenAppService))]
namespace KegID.UWP.DependencyServices
{
    public class OpenAppService : IOpenAppService
    {
        public async Task<bool> Launch(string stringUri)
        {
            Uri uri = new Uri(stringUri);
            return await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }

}
