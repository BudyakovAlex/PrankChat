using System;
using System.Threading.Tasks;
using Foundation;
using PrankChat.Mobile.iOS.Plugins.Logging;

namespace PrankChat.Mobile.iOS.Plugins.HttpClient.Delegates
{
    public class UploadVideoUrlSessionDataDelegate : NSObject, INSUrlSessionDataDelegate
    {
        private readonly TaskCompletionSource<string> _taskCompletionSource;
        private readonly Action<double, double> _onChangedProgressAction;

        private NSData _receiveData;

        public UploadVideoUrlSessionDataDelegate(TaskCompletionSource<string> taskCompletionSource, Action<double, double> onChangedProgressAction = null)
        {
            _taskCompletionSource = taskCompletionSource;
            _onChangedProgressAction = onChangedProgressAction;
        }

        [Export("URLSession:dataTask:didReceiveData:")]
        public void DidReceiveData(NSUrlSession session, NSUrlSessionDataTask dataTask, NSData data)
        {
            _receiveData = data;
            NativeConsoleLogger.Write("PRANKCHAT", $"didReceiveData    {data}");
        }

        [Export("URLSession:task:didSendBodyData:totalBytesSent:totalBytesExpectedToSend:")]
        public void DidSendBodyData(NSUrlSession session, NSUrlSessionTask task, long bytesSent, long totalBytesSent, long totalBytesExpectedToSend)
        {
            _onChangedProgressAction?.Invoke(totalBytesSent, totalBytesExpectedToSend);
            NativeConsoleLogger.Write("PRANKCHAT", $"didSendBody   {bytesSent}   {totalBytesSent}    {totalBytesExpectedToSend}");
        }

        [Export("URLSession:task:didCompleteWithError:")]
        public void DidCompleteWithError(NSUrlSession session, NSUrlSessionTask task, NSError error)
        {
            NativeConsoleLogger.Write("PRANKCHAT", $"didCompleteWithError    {error}");

            if (_receiveData != null)
            {
                if (_taskCompletionSource.TrySetResult(_receiveData.ToString()))
                {
                    return;
                }

                return;
            }

            _taskCompletionSource.TrySetException(new Exception(error?.LocalizedDescription ?? "no data"));
        }
    }
}
