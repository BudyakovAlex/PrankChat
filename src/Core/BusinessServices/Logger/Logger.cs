using PrankChat.Mobile.Core.Infrastructure.Extensions;
using System;
using System.IO;
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

        public Task<string> ExtractAndClearLogContentAsync()
        {
            return _semaphoreSlim.WrapAsync(() =>
            {
                try
                {
                    var filePath = Path.Combine(_applicationFolder, LogFileName);
                    var content = File.ReadAllText(filePath);
                    File.WriteAllText(filePath, string.Empty);

                    return Task.FromResult(content);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    return null;
                }
            });
        }

        public Task WriteRequestInfoAsync(DateTime dateTime, string tag, string message, bool isEndOfRequest = false, string parameters = "")
        {
            var body = ProduceMessageText(dateTime, isEndOfRequest, tag, message, parameters);
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

        private string ProduceMessageText(DateTime dateTime, bool isEndOfRequest, string tag, string message, string parameters)
        {
            parameters = string.IsNullOrWhiteSpace(parameters) ? "No parameters" : parameters;
            var header = isEndOfRequest ? "REQUEST ENDED AT:" : "REQUEST STARTED AT:";
            var footer = isEndOfRequest ? "[RESPONSE]" : "[PARAMETERS]";
            var sb = new StringBuilder();
            sb.AppendLine()
              .AppendLine("--------------------------------")
              .AppendLine($"[{header}] [{dateTime:u}]")
              .AppendLine($"[{tag}] {message}")
              .AppendLine()
              .AppendLine(footer)
              .AppendLine(parameters);

            return sb.ToString();
        }
    }
}
