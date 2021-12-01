using System;
using System.IO;
using System.Text;
using Java.IO;
using Square.OkIO;

namespace PrankChat.Mobile.Droid.Plugins.HttpClient.Builders
{
    public class FormDataBuilder
    {
        public const string ContentTypeKey = "Content-Type";
        public const string DefaultBoundary = "----PrankChatBoundary7MA4YWxkTrZu0gW";
        public const string MultipartSuffixTemplate = "form-data; name=\"{0}\"";
        private const string ContentTypeAppJson = "application/json";
        private const string ContentDespositionKey = "Content-Disposition";
        private const string DispositionTypeFormData = "form-data";
        private const string NewLine = "\r\n";
        private readonly static string ContentDispositionDefaultTemplate = $@"{ContentDespositionKey}: {DispositionTypeFormData}; name=""{{0}}""{NewLine}";
        private readonly static string ContentDispositionFileTemplate = $@"{ContentDespositionKey}: {DispositionTypeFormData}; filename={{0}}; name={{1}}{NewLine}";
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
            _multipartFormDataContent.Append($"{GetRequestBoundary()}{NewLine}");
            _multipartFormDataContent.Append($"{ContentTypeKey}: {ContentTypeAppJson}{NewLine}");
            _multipartFormDataContent.Append(string.Format(ContentDispositionDefaultTemplate, name));
            _multipartFormDataContent.Append(NewLine);
            _multipartFormDataContent.Append($"{value}{NewLine}");
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
            _multipartFormDataContent.Append($"{NewLine}{GetRequestBoundary()}{NewLine}");
            _multipartFormDataContent.Append(string.Format(ContentDispositionFileTemplate, Path.GetFileName(_filePath), _fileKeyName));
            _multipartFormDataContent.Append(NewLine);

            var charset = Java.Nio.Charset.Charset.DefaultCharset();
            outputStream.WriteString(_multipartFormDataContent.ToString(), charset);
            
            var fileInputStream = new FileInputStream(_filePath);
            int bytesRead;
            int totalWritedBytes = 0;
            do
            {
                var bytesAvailable = fileInputStream.Available();
                var bufferSize = Math.Min(bytesAvailable, MaxBufferSize);
                var buffer = new byte[bufferSize];
                bytesRead = fileInputStream.Read(buffer, 0, bufferSize);
                totalWritedBytes += bytesRead;
                outputStream.Write(buffer);
                outputStream.Flush();
                onChangedProgressAction?.Invoke(totalWritedBytes, fileInputStream.Available());
            }
            while (bytesRead > 0);

            fileInputStream.Dispose();
            outputStream.WriteString($"{NewLine}{GetRequestBoundary()}--{NewLine}", charset);
        }

        private string GetRequestBoundary()
             => $"--{DefaultBoundary}";
    }
}
