using System;
using System.Runtime.CompilerServices;
using Serilog;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class LoggerExtensions
    {
        private const string TagKey = "Tag";
        private const string TagDefault = "Common Info:";
        private const string TagFatal = "Unhandled Exception";
        private const string UnknownMember = "Unknown MemberName";
        private const string ExceptionTemplate = "Error logged in {FileName}->{MemberName}:{LineNumber} \r\n With message:{Message} \r\n {StackTrace}";

        public static ILogger Logger(this object? obj)
            => Log.Logger.WithTag($"{(obj is Type type ? type : obj?.GetType())?.Name ?? TagDefault}:");

        public static ILogger WithTag(this ILogger logger, string tag)
            => logger.ForContext(TagKey, tag);

        public static ILogger WithTag(this ILogger logger, object? tag)
            => logger.WithTag($"{tag ?? TagDefault}");

        public static void LogError(
            this ILogger logger,
            Exception? exception,
            string message = null,
            [CallerMemberName] string? memberName = null,
            [CallerFilePath] string? fileName = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            memberName ??= UnknownMember;
            fileName = GetFileName(fileName);
            logger.Error(exception, ExceptionTemplate, fileName, memberName, lineNumber, message, exception?.StackTrace);
        }

        public static void Fatal(
            this ILogger logger,
            Exception? exception,
            string message = null,
            [CallerMemberName] string? memberName = null,
            [CallerFilePath] string? fileName = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            memberName ??= UnknownMember;
            fileName = GetFileName(fileName);
            logger.WithTag(TagFatal)
                .Fatal(exception, ExceptionTemplate, exception?.Message, fileName, memberName, lineNumber, message, exception?.StackTrace);
        }

        public static void LogDebug(
            this ILogger logger,
            string message = null,
            [CallerMemberName] string? memberName = null,
            [CallerFilePath] string? fileName = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            memberName ??= UnknownMember;
            fileName = GetFileName(fileName);
            logger.Information("Message: {message} \r\n Invoked {FileName}->{MemberName}:{LineNumber}", message, fileName, memberName, lineNumber);
        }

        private static string GetFileName(string? filePath)
            => string.IsNullOrWhiteSpace(filePath) ? "Unknown FilePath" : System.IO.Path.GetFileName(filePath);
    }
}
