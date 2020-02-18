using System;
using PrankChat.Mobile.Core.BusinessServices.CrashlyticService;
using Firebase.Crashlytics;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Foundation;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.Crashlytic
{
    public class CrashlyticsService : ICrashlyticsService
    {
		private static readonly Regex _stackTraceRegex = new Regex(@"\s*at\s*(\S+)\.(\S+\(?.*\)?)\s\[0x[\d\w]+\]\sin\s.+[\\/>](.*):(\d+)?");

		public void TrackEvent(string message)
		{
            Crashlytics.SharedInstance.LogEvent(message);
        }

		public void TrackError(Exception exception)
		{
			if (exception is NSErrorException nsErrorException)
			{
				Crashlytics.SharedInstance.RecordError(nsErrorException.Error);
				return;
			}

			Crashlytics.SharedInstance.SetValue(exception.StackTrace, "Stack Trace");
			Crashlytics.SharedInstance.SetValue(exception.GetType().FullName, "Exception");
			Crashlytics.SharedInstance.RecordCustomExceptionName(exception.GetType().Name, exception.Message, CreateStackTrace(exception));
		}

		private static StackFrame[] CreateStackTrace(Exception exception)
		{
			var stackFrames = new List<StackFrame>();

			if (exception.StackTrace != null)
			{
				foreach (Match match in _stackTraceRegex.Matches(exception.StackTrace))
				{
					var cls = match.Groups[1].Value;
					var method = match.Groups[2].Value;
					var file = match.Groups[3].Value;
					var line = Convert.ToInt32(match.Groups[4].Value);
					if (!cls.StartsWith("System.Runtime.ExceptionServices", StringComparison.Ordinal) &&
						!cls.StartsWith("System.Runtime.CompilerServices", StringComparison.Ordinal))
					{
						if (string.IsNullOrEmpty(file))
							file = "filename unknown";

						var frame = StackFrame.Create(cls + "." + method);
						frame.FileName = file;
						frame.LineNumber = (uint)line;
						stackFrames.Add(frame);
					}
				}
			}

			return stackFrames.ToArray();
		}
	}
}
