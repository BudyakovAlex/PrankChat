using System;
using System.IO;
using System.Text;
using Java.IO;
using PrankChat.Mobile.Core.Common;
using Square.OkIO;

namespace PrankChat.Mobile.Droid.Plugins.HttpClient.Builders
{
    public class FormDataBuilder
    {
        private const int MaxBufferSize = 1024 * 1024;

        private static FormDataBuilder _instance;

        private string _filePath;
        private string _fileKeyName;
        private StringBuilder _multipartFormDataContent;

        private FormDataBuilder()
        {
            _multipartFormDataContent = new StringBuilder();
        }

        public static FormDataBuilder Create()
        {
            _instance = new FormDataBuilder();
            return _instance;
        }

        public FormDataBuilder AttachStringContent(string name, string value)
        {
            _multipartFormDataContent.Append($"{GetRequestBoundary()}{RestConstants.NewLine}");
            _multipartFormDataContent.Append($"{RestConstants.ContentTypeKey}: {RestConstants.ContentTypeAppJson}{RestConstants.NewLine}");
            _multipartFormDataContent.Append(string.Format(RestConstants.ContentDispositionDefaultTemplate, name));
            _multipartFormDataContent.Append(RestConstants.NewLine);
            _multipartFormDataContent.Append($"{value}{RestConstants.NewLine}");
            return this;
        }

        public FormDataBuilder AttachFileContent(string name, string filePath)
        {
            _filePath = filePath;
            _fileKeyName = name;
            return this;
        }

        public void WriteTo(IBufferedSink outputStream, Action<double, double> onChangedProgressAction)
        {
            _multipartFormDataContent.Append($"{RestConstants.NewLine}{GetRequestBoundary()}{RestConstants.NewLine}");
            _multipartFormDataContent.Append(string.Format(RestConstants.ContentDispositionFileTemplate, Path.GetFileName(_filePath), _fileKeyName));
            _multipartFormDataContent.Append(RestConstants.NewLine);

            var charset = Java.Nio.Charset.Charset.DefaultCharset();
            outputStream.WriteString(_multipartFormDataContent.ToString(), charset);
            
            var fileInputStream = new FileInputStream(_filePath);
            var bytesRead = 0;
            var totalWritedBytes = 0;
            do
            {
                var bytesAvailable = fileInputStream.Available();
                var bufferSize = Math.Min(bytesAvailable, MaxBufferSize);
                var buffer = new byte[bufferSize];

                bytesRead = fileInputStream.Read(buffer, 0, bufferSize);
                outputStream.Write(buffer);

                outputStream.Flush();

                totalWritedBytes += bytesRead;
                onChangedProgressAction?.Invoke(totalWritedBytes, fileInputStream.Available());
            }
            while (bytesRead > 0);

            fileInputStream.Dispose();
            outputStream.WriteString($"{RestConstants.NewLine}{GetRequestBoundary()}--{RestConstants.NewLine}", charset);
        }

        private string GetRequestBoundary()
             => $"--{RestConstants.DefaultBoundary}";
    }
}
