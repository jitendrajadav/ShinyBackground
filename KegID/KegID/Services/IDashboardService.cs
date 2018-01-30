﻿using KegID.Model;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IDashboardService
    {
        Task<DashboardModel> GetDeshboardDetailAsync(string sessionId);
    }
}
