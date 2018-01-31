using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;

namespace KegID.Services
{
    public class DashboardService : IDashboardService
    {
        public async Task<DashboardModel> GetDeshboardDetailAsync(string sessionId)
        {
            DashboardModel dashboardModel = new DashboardModel();

            string url = string.Format(Configuration.GetDashboardUrl, sessionId);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            dashboardModel = Helper.DeserializeObject<DashboardModel>(value.Response);
            dashboardModel.StatusCode = value.StatusCode;
            return dashboardModel;
        }
    }
}
