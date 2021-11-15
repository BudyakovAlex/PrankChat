using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using UIKit;
using Microsoft.AppCenter.Crashes;

namespace PrankChat.Mobile.iOS
{
    public class Application
    {
        private delegate void NsUncaughtExceptionHandler(IntPtr exception);

        [DllImport("/System/Library/Frameworks/Foundation.framework/Foundation")]
        private static extern void NSSetUncaughtExceptionHandler(IntPtr handler);

        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            NSSetUncaughtExceptionHandler(Marshal.GetFunctionPointerForDelegate(new NsUncaughtExceptionHandler(UncaughtExceptionHandler)));
            Runtime.MarshalManagedException += Runtime_MarshalManagedException;
            Runtime.MarshalObjectiveCException += Runtime_MarshalObjectiveCException;

            UIApplication.Main(args, null, "AppDelegate");
        }

        private static void Runtime_MarshalObjectiveCException(object sender, MarshalObjectiveCExceptionEventArgs args)
            => Crashes.TrackError(new MonoTouchException(args.Exception));

        private static void Runtime_MarshalManagedException(object sender, MarshalManagedExceptionEventArgs args)
            => Crashes.TrackError(args.Exception);

        [MonoPInvokeCallback(typeof(NsUncaughtExceptionHandler))]
        private static void UncaughtExceptionHandler(IntPtr exception)
        {
            try
            {
                Crashes.TrackError(new MonoTouchException(new IntPtrNsException(exception)));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private class IntPtrNsException : NSException
        {
            public IntPtrNsException(IntPtr handle)
                : base(handle)
            {
            }
        }
    }
}