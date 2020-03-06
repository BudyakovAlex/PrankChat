using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Crashlytics;
using Foundation;
using UIKit;

namespace PrankChat.Mobile.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            try
            {
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception exception)
            {
                var crashInfo = new Dictionary<object, object>
                {
                    [NSError.LocalizedDescriptionKey] = exception.Message,
                    ["StackTrace"] = exception.StackTrace
                };

                var error = new NSError(new NSString(exception.GetType().FullName), -1, NSDictionary.FromObjectsAndKeys(crashInfo.Values.ToArray(), crashInfo.Keys.ToArray(), crashInfo.Count));
                Crashlytics.SharedInstance.RecordError(error);
            }
        }
    }
}