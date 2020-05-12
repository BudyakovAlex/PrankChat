using System;
using Android.Content;
using Android.Util;
using Android.Views;

namespace PrankChat.Mobile.Droid.Controls
{
    public class AutoFitTextureView : TextureView
    {
        private int ratioWidth = 0;
        private int ratioHeight = 0;

        public AutoFitTextureView(Context context) : this(context, null)
        {
        }

        public AutoFitTextureView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public AutoFitTextureView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        public void SetAspectRatio(int width, int height)
        {
            if (width < 0 || height < 0)
            {
                throw new ArgumentException("Size cannot be negative.");
            }

            ratioWidth = width;
            ratioHeight = height;

            RequestLayout();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var width = MeasureSpec.GetSize(widthMeasureSpec);
            var height = MeasureSpec.GetSize(heightMeasureSpec);

            if (0 == ratioWidth ||
                0 == ratioHeight)
            {
                SetMeasuredDimension(width, height);
                return;
            }

            if (width < height * ratioWidth / ratioHeight)
            {
                SetMeasuredDimension(width, width * ratioHeight / ratioWidth);
                return;
            }

            SetMeasuredDimension(height * ratioWidth / ratioHeight, height);
        }
    }
}