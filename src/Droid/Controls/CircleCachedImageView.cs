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
        private TextPaint _textPaint;

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
            DownsampleHeight = 50;
            DownsampleWidth = 50;

            SetScaleType(ScaleType.CenterCrop);
            SetBackgroundResource(Resource.Drawable.ic_image_background);
        }

        protected override void OnDraw(Canvas canvas)
        {
            var radius = Height / 2;
            var path = new Path();
            var rect = new RectF(0, 0, Width, Height);
            path.AddRoundRect(rect, radius, radius, Path.Direction.Cw);
            canvas.ClipPath(path);

            DrawText(canvas);

            base.OnDraw(canvas);
        }

        private void DrawText(Canvas canvas)
        {
            if (string.IsNullOrWhiteSpace(PlaceholderText))
                return;

            _textPaint = new TextPaint
            {
                Color = Color.White,
                TextSize = 16 * Resources.DisplayMetrics.Density,
                TextAlign = Paint.Align.Center,
            };
            var textBounds = new Rect();
            _textPaint.GetTextBounds(PlaceholderText, 0, PlaceholderText.Length, textBounds);
            _textPaint.SetTypeface(Typeface.Create(Typeface.Default, TypefaceStyle.Bold));

            var textDrawX = Width / 2;
            var textDrawY = (int)((Height / 2) - ((_textPaint.Descent() + _textPaint.Ascent()) / 2));
            canvas.DrawText(PlaceholderText, textDrawX, textDrawY, _textPaint);
        }
    }
}
