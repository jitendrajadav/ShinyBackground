using Acr.UserDialogs;
using System;
using Xamarin.Forms;

namespace KegID.Services
{
    public class Loader : ILoader
    {
        public void StartLoading(string message = "Loading...")
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

        public void StopLoading()
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
