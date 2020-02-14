using System;
using Android.Views;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    public class ViewOnClickListener : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Action<View> onClickAction;

        public ViewOnClickListener(Action<View> onClickAction)
        {
            this.onClickAction = onClickAction;
        }

        public void OnClick(View v)
        {
            onClickAction?.Invoke(v);
        }
    }
}
