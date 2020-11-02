using UIKit;
using Sentry;

namespace PrankChat.Mobile.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {

            using (SentrySdk.Init("https://077bd6b9706f4247a2bf8cab4ac3bf44@o453054.ingest.sentry.io/5454419"))
            {

            }   
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");

        }
    }
}