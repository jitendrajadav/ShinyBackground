using System;
using System.Threading;
using System.Threading.Tasks;
using KegID.Messages;
using UIKit;
using Xamarin.Forms;

namespace KegID.iOS.Services
{
    public class iOSLongRunningTaskExample
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start(StartLongRunningTaskMessage startLongRunningTaskMessage)
        {
            _cts = new CancellationTokenSource();

            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("LongRunningTask", OnExpiration);

            try
            {
                //INVOKE THE SHARED CODE
                var counter = new TaskCounter();
                await counter.RunCounter(_cts.Token, startLongRunningTaskMessage.Barcode);

            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (_cts.IsCancellationRequested)
                {
                    var message = new CancelledMessage();
                    Device.BeginInvokeOnMainThread(
                        () => MessagingCenter.Send(message, "CancelledMessage")
                    );
                }
            }

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        void OnExpiration()
        {
            _cts.Cancel();
        }
    }
}