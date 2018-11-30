﻿namespace KegID.Services
{
    public interface ILoader
    {
        void StartLoading(string message = "Loading...");
        void StopLoading();
        void Toast(string msg);
    }
}
