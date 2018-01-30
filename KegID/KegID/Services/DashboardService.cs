using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;

namespace KegID.Services
{
    public class DashboardService : IDashboardService
    {
        public async Task<DashboardModel> GetDeshboardDetailAsync(string sessionId)
        {
            string url = string.Format(Configuration.GetDashboardUrl, sessionId);
            return await Helper.ExecuteServiceCall<DashboardModel>(url, HttpMethodType.Get, string.Empty);
        }
    }
}
