using System;
using Android.Graphics;
using Android.Views;

namespace PrankChat.Mobile.Droid.Providers
{
    public class OutlineProvider : ViewOutlineProvider
    {
        private readonly Action<View, Outline> _getOutline;

        public OutlineProvider(Action<View, Outline> getOutline)
        {
            _getOutline = getOutline;
        }

        public override void GetOutline(View view, Outline outline)
        {
            _getOutline(view, outline);
        }
    }
}