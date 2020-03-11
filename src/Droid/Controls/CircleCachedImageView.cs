using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Util;
using FFImageLoading.Cross;

namespace PrankChat.Mobile.Droid.Controls
{
    public class CircleCachedImageView : MvxCachedImageView
    {
        private const int ImageSize = 50;
        private const int PlaceholderTextSize = 16;

        private TextPaint _placeholderPaint;

        public string PlaceholderText { get; set; }

        public CircleCachedImageView(Context context) : base(context)
        {
            Initilize();
        }

        public CircleCachedImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initilize();
        }

        public CircleCachedImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initilize();
        }

        private void Initilize()
        {
            DownsampleHeight = ImageSize;
            DownsampleWidth = ImageSize;

            SetScaleType(ScaleType.CenterCrop);
            SetBackgroundResource(Resource.Drawable.ic_notification_user);
        }

        protected override void OnDraw(Canvas canvas)
        {
            var radius = Height / 2;
            var path = new Path();
            var rect = new RectF(0, 0, Width, Height);
            path.AddRoundRect(rect, radius, radius, Path.Direction.Cw);
            canvas.ClipPath(path);

            DrawPlaceholder(canvas);

            base.OnDraw(canvas);
        }

        private void DrawPlaceholder(Canvas canvas)
        {
            if (string.IsNullOrWhiteSpace(PlaceholderText))
                return;

            _placeholderPaint = new TextPaint
            {
                Color = Color.White,
                TextSize = PlaceholderTextSize * Resources.DisplayMetrics.Density,
                TextAlign = Paint.Align.Center,
            };
            var textBounds = new Rect();
            _placeholderPaint.GetTextBounds(PlaceholderText, 0, PlaceholderText.Length, textBounds);
            _placeholderPaint.SetTypeface(Typeface.Create(Typeface.Default, TypefaceStyle.Bold));

            var textDrawX = Width / 2;
            var textDrawY = (int)((Height / 2) - ((_placeholderPaint.Descent() + _placeholderPaint.Ascent()) / 2));
            canvas.DrawText(PlaceholderText, textDrawX, textDrawY, _placeholderPaint);
        }
    }
}
