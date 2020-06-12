using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using System;

namespace PrankChat.Mobile.Droid.Controls
{
    public class CircleProgressBar : View
    {
        public CircleProgressBar(Context context) : base(context)
        {
            SetWillNotDraw(false);
        }

        public CircleProgressBar(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SetWillNotDraw(false);
        }

        public CircleProgressBar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            SetWillNotDraw(false);
        }

        public CircleProgressBar(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            SetWillNotDraw(false);
        }

        protected CircleProgressBar(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            SetWillNotDraw(false);
        }

        private float _ringThickness;
        public float RingThickness
        {
            get => _ringThickness;
            set
            {
                _ringThickness = value;
                Invalidate();
            }
        }

        private float _progress;
        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                Invalidate();
            }
        }

        private Color _baseColor;
        public Color BaseColor
        {
            get => _baseColor;
            set
            {
                _baseColor = value;
                Invalidate();
            }
        }

        private Color _progressColor;
        public Color ProgressColor
        {
            get => _progressColor;
            set
            {
                _progressColor = value;
                Invalidate();
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            var displayDensity = Context.Resources.DisplayMetrics.Density;
            var strokeWidth = (float)Math.Ceiling(RingThickness * displayDensity);

            var paint = new Paint
            {
                StrokeWidth = strokeWidth
            };

            paint.SetStyle(Paint.Style.Stroke);
            paint.Flags = PaintFlags.AntiAlias;

            var ringAreaSize = Math.Min(canvas.ClipBounds.Width(), canvas.ClipBounds.Height());

            var ringDiameter = ringAreaSize - paint.StrokeWidth;

            var left = canvas.ClipBounds.CenterX() - ringDiameter / 2;
            var top = canvas.ClipBounds.CenterY() - ringDiameter / 2;

            var ringDrawArea = new RectF(left, top, left + ringDiameter, top + ringDiameter);
            DrawProgressRing(canvas, Progress / 100, BaseColor, ProgressColor, ringDrawArea, paint);
        }

        private void DrawProgressRing(Canvas canvas,
                                      float progress,
                                      Color ringBaseColor,
                                      Color ringProgressColor,
                                      RectF ringDrawArea,
                                      Paint paint)
        {
            paint.Color = ringBaseColor;
            canvas.DrawArc(ringDrawArea, 270, 360, false, paint);

            paint.Color = ringProgressColor;
            canvas.DrawArc(ringDrawArea, 270, 360 * progress, false, paint);
        }
    }
}
