using Acr.UserDialogs;
using System;
using Xamarin.Forms;

namespace KegID.Common
{
    public class Loader
    {
        public static void StartLoading(string message = "Loading...")
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.ShowLoading(message);
                });
            }
            catch (Exception e)
            {
                new Exception(e.Message);
            }
        }

        public static void StopLoading()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                });
            }
            catch (Exception e)
            {
                new Exception(e.Message);
            }
        }
    }
}
