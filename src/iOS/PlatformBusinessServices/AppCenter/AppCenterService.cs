using Firebase.Crashlytics;
using Foundation;
using PrankChat.Mobile.Core.BusinessServices.AppCenter;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.AppCenter
{
    public class AppCenterService : IAppCenterService
    {
		private static readonly Regex _stackTraceRegex = new Regex(@"\s*at\s*(\S+)\.(\S+\(?.*\)?)\s\[0x[\d\w]+\]\sin\s.+[\\/>](.*):(\d+)?");

		public void TrackEvent(string message)
		{
            Crashlytics.SharedInstance.Log(message);
        }

		public void TrackError(Exception exception)
		{
			if (exception is NSErrorException nsErrorException)
			{
				Crashlytics.SharedInstance.RecordError(nsErrorException.Error);
				return;
			}

			Crashlytics.SharedInstance.SetCustomValue(NSObject.FromObject(exception.StackTrace), "Stack Trace");
			Crashlytics.SharedInstance.SetCustomValue(NSObject.FromObject(exception.GetType().FullName), "Exception");
            var error = new ExceptionModel(exception.GetType().FullName, exception.StackTrace)
            {
                StackTrace = CreateStackTrace(exception)
            };

            Crashlytics.SharedInstance.RecordExceptionModel(error);
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

						var frame = StackFrame.Create(cls + "." + method, file, line);
						stackFrames.Add(frame);
					}
				}
			}

			return stackFrames.ToArray();
		}
	}
}
