﻿using KegID.Model;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IAccountService
    {
        Task<LoginModel> AuthenticateAsync(string username, string password);
    }
}
