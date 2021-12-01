using System.IO;
using Foundation;
using PrankChat.Mobile.Core.Common;

namespace PrankChat.Mobile.iOS.Plugins.HttpClient.Builders
{
    public class FormDataBuilder
    {
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
            _multipartFormDataContent.AppendData($"{GetRequestBoundary()}{RestConstants.NewLine}");
            _multipartFormDataContent.AppendData($"{RestConstants.ContentTypeKey}: {RestConstants.ContentTypeAppJson}{RestConstants.NewLine}");
            _multipartFormDataContent.AppendData(string.Format(RestConstants.ContentDispositionDefaultTemplate, name));
            _multipartFormDataContent.AppendData(RestConstants.NewLine);
            _multipartFormDataContent.AppendData($"{value}{RestConstants.NewLine}");
            return _instance;
        }

        public FormDataBuilder AttachFileContent(string name, string filePath)
        {
            _multipartFormDataContent.AppendData($"{RestConstants.NewLine}{GetRequestBoundary()}{RestConstants.NewLine}");
            _multipartFormDataContent.AppendData(string.Format(RestConstants.ContentDispositionFileTemplate, Path.GetFileName(filePath), name));
            _multipartFormDataContent.AppendData(RestConstants.NewLine);

            var fileData = NSData.FromFile(filePath);
            _multipartFormDataContent.AppendData(fileData);
            return _instance;
        }

        public NSData Build()
        {
            _multipartFormDataContent.AppendData($"{RestConstants.NewLine}{GetRequestBoundary()}--{RestConstants.NewLine}");
            return _multipartFormDataContent;
        }

        private string GetRequestBoundary()
             => $"--{RestConstants.DefaultBoundary}";
    }
}
