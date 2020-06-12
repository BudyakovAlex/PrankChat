using CoreGraphics;
using Foundation;
using System;
using System.ComponentModel;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register("CircleProgressBar"), DesignTimeVisible(true)]
    public class CircleProgressBar : UIView
    {
        public CircleProgressBar()
        {
        }

        public CircleProgressBar(NSCoder coder) : base(coder)
        {
        }

        protected CircleProgressBar(NSObjectFlag t) : base(t)
        {
        }

        protected internal CircleProgressBar(IntPtr handle) : base(handle)
        {
        }

        public CircleProgressBar(CGRect frame) : base(frame)
        {
        }

        private float _ringThickness;
        public float RingThickness
        {
            get => _ringThickness;
            set
            {
                _ringThickness = value;
                SetNeedsDisplay();
            }
        }

        private float _progress;
        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                SetNeedsDisplay();
            }
        }

        private UIColor _baseColor;
        public UIColor BaseColor
        {
            get => _baseColor;
            set
            {
                _baseColor = value;
                SetNeedsDisplay();
            }
        }

        private UIColor _progressColor;

        public UIColor ProgressColor
        {
            get => _progressColor;
            set
            {
                _progressColor = value;
                SetNeedsDisplay();
            }
        }

        private nfloat GetRadius(nfloat lineWidth)
        {
            var width = Bounds.Width;
            var height = Bounds.Height;
            var size = (float)Math.Min(width, height);

            var radius = (size / 2f) - ((float)lineWidth / 2f);
            return radius;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            using (CGContext graphics = UIGraphics.GetCurrentContext())
            {
                var lineWidth = RingThickness;
                var radius = (int)GetRadius(lineWidth);

                var progressFromPercentage = Progress / 100;
                DrawProgressRing(graphics, Bounds.GetMidX(), Bounds.GetMidY(), progressFromPercentage, lineWidth, radius, BaseColor, ProgressColor);
            };
        }

        // TODO Optimize circle drawing by removing allocation of CGPath
        // (maybe by drawing via BitmapContext, per pixel:
        // https://stackoverflow.com/questions/34987442/drawing-pixels-on-the-screen-using-coregraphics-in-swift)
        private void DrawProgressRing(CGContext graphics, nfloat x0, nfloat y0,
                                     nfloat progress, nfloat lineThickness, nfloat radius,
                                     UIColor backColor, UIColor frontColor)
        {
            graphics.SetLineWidth(lineThickness);

            // Draw background circle
            var path = new CGPath();
            backColor.SetStroke();

            path.AddArc(x0, y0, radius, 0, 2.0f * (float)Math.PI, true);
            graphics.AddPath(path);
            graphics.DrawPath(CGPathDrawingMode.Stroke);

            // Draw progress circle
            var pathStatus = new CGPath();
            frontColor.SetStroke();

            var startingAngle = 1.5f * (float)Math.PI;
            pathStatus.AddArc(x0, y0, radius, startingAngle, startingAngle + progress * 2 * (float)Math.PI, false);

            graphics.AddPath(pathStatus);
            graphics.DrawPath(CGPathDrawingMode.Stroke);
        }
    }
}
