using System;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace PrankChat.Mobile.Droid.Presenters.Attributes
{
    /// <summary>
    /// Use this attribute if need force clear previous activity and navigation stack
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ClearStackActivityPresentationAttribute : MvxActivityPresentationAttribute
    {
    }
}