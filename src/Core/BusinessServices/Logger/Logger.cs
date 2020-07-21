using PrankChat.Mobile.Core.Infrastructure.Extensions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.BusinessServices.Logger
{
    public class Logger : ILogger
    {
        private const string LogFileName = "Log.txt";
        
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly string _applicationFolder;

        public Logger()
        {
            _applicationFolder = Xamarin.Essentials.FileSystem.AppDataDirectory;
            CreateFileIfNotExists();
        }

        public string LogFilePath => Path.Combine(_applicationFolder, LogFileName);

        public Task ClearLogAsync()
        {
            return _semaphoreSlim.WrapAsync(() =>
            {
                try
                {
                    var filePath = Path.Combine(_applicationFolder, LogFileName);
                    File.WriteAllText(filePath, string.Empty);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    return;
                }
            });
        }

        public Task LogEventAsync(DateTime dateTime, string tag, string extraParameters = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine()
              .AppendLine("--------------------------------")
              .AppendLine($"[{dateTime:u}] [{tag}]");

            if (!string.IsNullOrWhiteSpace(extraParameters))
            {
                sb.AppendLine($"[{dateTime:u}] [{tag}]");
            }

            return _semaphoreSlim.WrapAsync(() =>
            {
                try
                {
                    var filePath = Path.Combine(_applicationFolder, LogFileName);
                    File.AppendAllText(filePath, sb.ToString());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
        }

        public Task WriteRequestInfoAsync(DateTime dateTime, string tag, string message, List<Parameter> parameters = null)
        {
            var body = ProduceRequestMessageText(dateTime, tag, message, parameters);
            return _semaphoreSlim.WrapAsync(() =>
            {
                try
                {
                    var filePath = Path.Combine(_applicationFolder, LogFileName);
                    File.AppendAllText(filePath, body);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
        }

        public Task WriteResponseInfoAsync(DateTime dateTime, HttpStatusCode statusCode, string tag, string message, string content, List<Parameter> headers = null)
        {
            var body = ProduceResponseMessageText(dateTime,statusCode, tag, message, content, headers);
            return _semaphoreSlim.WrapAsync(() =>
            {
                try
                {
                    var filePath = Path.Combine(_applicationFolder, LogFileName);
                    File.AppendAllText(filePath, body);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
        }

        private void CreateFileIfNotExists()
        {
            try
            {
                var filePath = Path.Combine(_applicationFolder, LogFileName);
                if (File.Exists(filePath))
                {
                    return;
                }

                File.Create(filePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private string ProduceRequestMessageText(DateTime dateTime, string tag, string message, List<Parameter> parameters)
        {
            var sb = new StringBuilder();
            sb.AppendLine()
              .AppendLine("--------------------------------")
              .AppendLine($"[REQUEST STARTED AT:] [{dateTime:u}]")
              .AppendLine($"[{tag}] {message}")
              .AppendLine("[HEADERS:]")
              .AppendLine(ProduceParametersStringByType(parameters, ParameterType.HttpHeader))
              .AppendLine("[PARAMETERS:]")
              .AppendLine(ProduceParametersStringByType(parameters, ParameterType.RequestBody));

            return sb.ToString();
        }

        private string ProduceResponseMessageText(DateTime dateTime, HttpStatusCode statusCode, string tag, string message, string content, List<Parameter> headers = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine()
              .AppendLine("--------------------------------")
              .AppendLine($"[REQUEST ENDED AT:] [{dateTime:u}]")
              .AppendLine($"[{(int)statusCode}] [{tag}] {message}")
              .AppendLine("[HEADERS:]")
              .AppendLine(ProduceParametersStringByType(headers, ParameterType.HttpHeader))
              .AppendLine("[CONTENT:]")
              .AppendLine(content);

            return sb.ToString();
        }

        private string ProduceParametersStringByType(List<Parameter> parameters, ParameterType parameterType)
        {
            if (parameters is null || parameters.Count == 0)
            {
                return "-";
            }

            var sb = new StringBuilder();
            var filteredParameters = parameters.Where(parameter => parameter.Type == parameterType).ToList();
            filteredParameters.ForEach(parameter =>
            {
                sb.AppendLine($"{parameter.Name} : {parameter.Value} - {parameter.ContentType};");
            });

            return sb.ToString();
        }
    }
}
