using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;

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