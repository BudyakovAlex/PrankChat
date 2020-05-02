using System;
using Android.Views;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    public static class ViewOnClickListenerExtensions
    {
        public static void SetOnClickListener(this View view, Action<View> action) =>
            view.SetOnClickListener(new ViewOnClickListener(action));
    } 

    public class ViewOnClickListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Action<View> _onClickAction;

        public ViewOnClickListener(Action<View> onClickAction)
        {
            _onClickAction = onClickAction;
        }

        public void OnClick(View v)
        {
            _onClickAction?.Invoke(v);
        }
    }
}
