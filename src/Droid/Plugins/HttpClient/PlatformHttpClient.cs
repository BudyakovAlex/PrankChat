using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Android.OS;
using Java.IO;
using Java.Net;
using Java.Util.Concurrent;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Core.Services.Network;

namespace PrankChat.Mobile.Droid.Plugins.HttpClient
{
    public class PlatformHttpClient : Java.Lang.Object, IPlatformHttpClient, IExecutor
    {
        private readonly IEnvironmentConfigurationProvider _environmentConfigurationProvider;
        private readonly IUserSessionProvider _userSessionProvider;

        public PlatformHttpClient(
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IUserSessionProvider userSessionProvider)
        {
            _environmentConfigurationProvider = environmentConfigurationProvider;
            _userSessionProvider = userSessionProvider;
        }

        public void Execute(Java.Lang.IRunnable command)
        {
        }

        public async Task<string> UploadVideoAsync(UploadVideoDto uploadVideoDto, Action<double, double> onChangedProgressAction = null)
        {
            var d = new BackgroundUploader(_environmentConfigurationProvider).Execute(uploadVideoDto);
            var k = await d.GetAsync();
            return "";
        }
    }

    public class BackgroundUploader : AsyncTask<UploadVideoDto, int, Task<string>>
    {
        private const string PostHttpMethod = "POST";
        private const string AuthorizationCookieKey = "Authorization";
        private const string AuthorizationCookieValueTemplate = "Bearer {0}";
        private const string ContentTypeCookieKey = "Accept-Language";
        private const string UrlStringTemplate = "{0}/api/v{1}/videos";
        private const string ContentTypeCookieValueTemplate = "multipart/form-data; boundary=\"{0}\"";

        private readonly IEnvironmentConfigurationProvider _environmentConfigurationProvider;

        public BackgroundUploader(IEnvironmentConfigurationProvider environmentConfigurationProvider)
        {
            _environmentConfigurationProvider = environmentConfigurationProvider;
        }

        protected override Task<string> RunInBackground(params UploadVideoDto[] @params)
        {
            var uploadVideoDto = @params[0];
            var environment = _environmentConfigurationProvider.Environment;
            var version = new Version(environment.ApiVersion);
            var currentCulture = CultureInfo.CurrentCulture;

            URL url = new URL(string.Format(UrlStringTemplate, environment.ApiUrl, version.Major));

            // Open a HTTP  connection to  the URL
            var connection = (HttpURLConnection)url.OpenConnection();
            connection.DoInput = true; // Allow Inputs
            connection.DoOutput = true; // Allow Outputs
            connection.UseCaches = false; // Don't use a Cached Copy
            connection.RequestMethod = PostHttpMethod;
            connection.SetRequestProperty("Connection", "Keep-Alive");
            connection.SetRequestProperty("ENCTYPE", "multipart/form-data");
            connection.SetRequestProperty("Content-Type", "multipart/form-data;boundary=" + "");

            var formDataBuilder = FormDataBuilder.Create(connection.InputStream);
            formDataBuilder.AttachStringContent("order_id", uploadVideoDto.OrderId.ToString())
                           .AttachStringContent("title", uploadVideoDto.Title)
                           .AttachStringContent("description", uploadVideoDto.Description)
                           .AttachFileContent("video", uploadVideoDto.FilePath);

            // Responses from the server (code and message)
            var serverResponseCode = connection.ResponseCode;
            var serverResponseMessage = connection.ResponseMessage;

            if (serverResponseCode == HttpStatus.Ok)
            {

            }

            formDataBuilder.Dispose();
            return Task.FromResult(serverResponseMessage);
        }

        protected override void OnProgressUpdate(params int[] values)
        {

        }
    }

    public class FormDataBuilder : IDisposable
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

        private static FormDataBuilder _instance;

        private DataOutputStream _multipartFormDataContent;
        private FileInputStream _fileInputStream;

        private FormDataBuilder(Stream stream)
        {
            _multipartFormDataContent = new DataOutputStream(stream);
        }

        public static FormDataBuilder Create(Stream stream)
        {
            _instance = new FormDataBuilder(stream);
            return _instance;
        }

        public FormDataBuilder AttachStringContent(string name, string value)
        {
            _multipartFormDataContent.WriteBytes($"{GetRequestBoundary()}{NewLine}");
            _multipartFormDataContent.WriteBytes($"{ContentTypeKey}: {ContentTypeAppJson}{NewLine}");
            _multipartFormDataContent.WriteBytes(string.Format(ContentDispositionDefaultTemplate, name));
            _multipartFormDataContent.WriteBytes(NewLine);
            _multipartFormDataContent.WriteBytes($"{value}{NewLine}");
            return _instance;
        }

        public FormDataBuilder AttachFileContent(string name, string filePath)
        {
            _multipartFormDataContent.WriteBytes($"{NewLine}{GetRequestBoundary()}{NewLine}");
            _multipartFormDataContent.WriteBytes(string.Format(ContentDispositionFileTemplate, Path.GetFileName(filePath), name));
            _multipartFormDataContent.WriteBytes(NewLine);

            _fileInputStream = new FileInputStream(filePath);
            // create a buffer of  maximum size
            var bytesAvailable = _fileInputStream.Available();

            var maxBufferSize = 1024 * 1024;
            var bufferSize = Math.Min(bytesAvailable, maxBufferSize);
            var buffer = new byte[bufferSize];

            // read file and write it into form...
            var bytesRead = _fileInputStream.Read(buffer, 0, bufferSize);

            while (bytesRead > 0)
            {
                _multipartFormDataContent.Write(buffer, 0, bufferSize);
                bytesAvailable = _fileInputStream.Available();
                bufferSize = Math.Min(bytesAvailable, maxBufferSize);
                bytesRead = _fileInputStream.Read(buffer, 0, bufferSize);
            }

            return _instance;
        }

        public DataOutputStream Build()
        {
            _multipartFormDataContent.WriteBytes($"{NewLine}{GetRequestBoundary()}--{NewLine}");
            return _multipartFormDataContent;
        }

        private string GetRequestBoundary()
             => $"--{DefaultBoundary}";

        public void Dispose()
        {
            _fileInputStream.Close();
            _multipartFormDataContent.Flush();
            _multipartFormDataContent.Close();
        }
    }
}
