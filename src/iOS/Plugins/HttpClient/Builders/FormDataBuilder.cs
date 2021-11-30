using System.IO;
using Foundation;

namespace PrankChat.Mobile.iOS.Plugins.HttpClient.Builders
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
        private static string ContentDispositionDefaultTemplate = $@"{ContentDespositionKey}: {DispositionTypeFormData}; name=""{{0}}""{NewLine}";
        private static string ContentDispositionFileTemplate = $@"{ContentDespositionKey}: {DispositionTypeFormData}; filename={{0}}; name={{1}}{NewLine}";

        private static FormDataBuilder _instance;

        private NSMutableData _multipartFormDataContent;

        private FormDataBuilder()
        {
            _multipartFormDataContent = new NSMutableData();
        }

        public static FormDataBuilder Create()
        {
            _instance = new FormDataBuilder();
            return _instance;
        }

        public FormDataBuilder AttachStringContent(string name, string value)
        {
            _multipartFormDataContent.AppendData($"{GetRequestBoundary()}{NewLine}");
            _multipartFormDataContent.AppendData($"{ContentTypeKey}: {ContentTypeAppJson}{NewLine}");
            _multipartFormDataContent.AppendData(string.Format(ContentDispositionDefaultTemplate, name));
            _multipartFormDataContent.AppendData(NewLine);
            _multipartFormDataContent.AppendData($"{value}{NewLine}");
            return _instance;
        }

        public FormDataBuilder AttachFileContent(string name, string filePath)
        {
            _multipartFormDataContent.AppendData($"{NewLine}{GetRequestBoundary()}{NewLine}");
            _multipartFormDataContent.AppendData(string.Format(ContentDispositionFileTemplate, Path.GetFileName(filePath), name));
            _multipartFormDataContent.AppendData(NewLine);

            var fileData = NSData.FromFile(filePath);
            _multipartFormDataContent.AppendData(fileData);
            return _instance;
        }

        public NSData Build()
        {
            _multipartFormDataContent.AppendData($"{NewLine}{GetRequestBoundary()}--{NewLine}");
            return _multipartFormDataContent;
        }

        private string GetRequestBoundary()
             => $"--{DefaultBoundary}";
    }
}
