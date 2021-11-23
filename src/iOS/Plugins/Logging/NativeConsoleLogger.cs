using System;
using Foundation;

namespace PrankChat.Mobile.iOS.Plugins.Logging
{
    public class NativeConsoleLogger
    {
        private const string FoundationLibrary = "/System/Library/Frameworks/Foundation.framework/Foundation";

        [System.Runtime.InteropServices.DllImport(FoundationLibrary)]
        private static extern void NSLog(IntPtr format, IntPtr s);

        [System.Runtime.InteropServices.DllImport(FoundationLibrary, EntryPoint = "NSLog")]
        private static extern void NSLog_ARM64(IntPtr format, IntPtr p2, IntPtr p3, IntPtr p4, IntPtr p5, IntPtr p6, IntPtr p7, IntPtr p8, IntPtr s);

        private static readonly bool IsDevice = ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.DEVICE;
        private static readonly bool Is64BitDevice = Environment.Is64BitOperatingSystem && IsDevice;
        private static readonly NSString NsFormat = new NSString(@"%@");

        public static void Write(string tag, string message)
        {
            try
            {
                var content = $"{tag}\n{message}";
                WriteToNativeLog(content);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private static void WriteToNativeLog(string text)
        {
            using (var nsText = new NSString(text))
            {
                WriteToNativeLog(nsText);
            }
        }

        private static void WriteToNativeLog(NSString text)
        {
            if (Is64BitDevice)
            {
                NSLog_ARM64(NsFormat.Handle, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, text.Handle);
                return;
            }

            NSLog(NsFormat.Handle, text.Handle);
        }
    }
}