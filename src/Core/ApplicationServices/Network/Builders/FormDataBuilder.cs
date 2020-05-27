﻿using System.Net.Http;
using System.Net.Http.Headers;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Builders
{
    public class FormDataBuilder
    {
        private const string ContentTypeKey = "Content-Type";
        private const string ContentTypeAppJson = "application/json";
        private const string DefaultBoundary = "----PrankChatBoundary7MA4YWxkTrZu0gW";
        private const string ContentDespositionKey = "Content-Disposition";
        private const string MultipartSuffixTemplate = "form-data; name=\"{0}\"";
        private const string DispositionTypeFormData = "form-data";

        private static FormDataBuilder _instance;

        private MultipartFormDataContent _multipartFormDataContent;
        private FormDataBuilder()
        {
            _multipartFormDataContent = new MultipartFormDataContent(DefaultBoundary);
        }

        public static FormDataBuilder Create()
        {
            _instance = new FormDataBuilder();
            return _instance;
        }

        public FormDataBuilder AttachStringContent(string name, string value)
        {
            var orderStringContent = new StringContent(value);
            orderStringContent.Headers.Remove(ContentTypeKey);
            orderStringContent.Headers.Add(ContentTypeKey, ContentTypeAppJson);
            orderStringContent.Headers.Add(ContentDespositionKey, string.Format(MultipartSuffixTemplate, name));
            _multipartFormDataContent.Add(orderStringContent, name);
            return _instance;
        }

        public FormDataBuilder AttachFileContent(string name, string fileName, byte[] content)
        {
            var fileContent = new ByteArrayContent(content);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(DispositionTypeFormData)
            {
                FileName = fileName,
                Name = name
            };

            _multipartFormDataContent.Add(fileContent, name);
            return _instance;
        }

        public MultipartFormDataContent Build()
        {
            return _multipartFormDataContent;
        }
    }
}
