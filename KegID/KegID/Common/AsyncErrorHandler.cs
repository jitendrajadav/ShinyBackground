using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using System;
using System.Diagnostics;

namespace KegID.Common
{
    public static class AsyncErrorHandler
    {
        public static string Message { get; set; }

        public static void HandleException(Exception ex)
        {
            Message = ex.Message;
            Crashes.TrackError(ex);
            UserDialogs.Instance.HideLoading();
        }
    }
}
