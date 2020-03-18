using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Controls
{
    public class SelectableButton : Button
    {
        private const string PositiveString = "positive";
        private const string NegativeString = "negative";
        private const int Padding = 10;

        private Drawable _drawableForSelectedState;
        private Drawable _drawableForUnselectedState;
        private Drawable _currentDrawable;
        private string _typeArbitrationButton;
        private int _horizontalPaddingInDp;

        private ArbitrationValueType? _arbitrationValue;
        public ArbitrationValueType? ArbitrationValue
        {
            get => _arbitrationValue;
            set
            {
                _arbitrationValue = value;
                ChangeSelectedState();
            }
        }

        #region Constructions

        protected SelectableButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        public SelectableButton(Context context) : base(context)
        {
            Initialize();
        }

        public SelectableButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(attrs);
        }

        public SelectableButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(attrs);
        }

        #endregion

        private void Initialize(IAttributeSet attrs = null)
        {
            if (attrs != null)
            {
                var array = Context.ObtainStyledAttributes(attrs, Resource.Styleable.SelectableButton, 0, 0);

                _drawableForSelectedState = array.GetDrawable(Resource.Styleable.SelectableButton_selected_drawable);
                _drawableForUnselectedState = array.GetDrawable(Resource.Styleable.SelectableButton_unselected_drawable);
                _typeArbitrationButton = array.GetNonResourceString(Resource.Styleable.SelectableButton_type_arbitration);

                array.Recycle();
            }
            _horizontalPaddingInDp = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, Padding, Resources.DisplayMetrics);
            ChangeSelectedState();
        }

        private void ChangeSelectedState()
        {
            if ((_arbitrationValue == ArbitrationValueType.Positive && _typeArbitrationButton == PositiveString) ||
                (_arbitrationValue == ArbitrationValueType.Negative && _typeArbitrationButton == NegativeString) ||
                _arbitrationValue == null)
            {
                SetStyle(true, 1f, _drawableForSelectedState);
            }
            else
            {
                SetStyle(false, 0.5f, _drawableForUnselectedState);
            }
        }

        private void SetStyle(bool enabled, float alpha, Drawable currentDrawable)
        {
            Enabled = enabled;
            Alpha = alpha;
            _currentDrawable = currentDrawable;
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            var textBounds = new Rect();
            Paint.GetTextBounds(Text, 0, Text.Length, textBounds);

            var fullWidth = _currentDrawable.IntrinsicWidth + _horizontalPaddingInDp + textBounds.Width();
            var leftSideIcon = canvas.Width / 2 - fullWidth / 2;
            var verticalCenter = canvas.Height / 2;
            var leftSideText = leftSideIcon + _currentDrawable.IntrinsicWidth + _horizontalPaddingInDp;

            _currentDrawable.SetBounds(leftSideIcon,
                                       verticalCenter - _currentDrawable.IntrinsicHeight / 2,
                                       leftSideText - _horizontalPaddingInDp,
                                       verticalCenter + _currentDrawable.IntrinsicHeight / 2);
            _currentDrawable.Draw(canvas);

            var textPaint = new Paint(PaintFlags.AntiAlias)
            {
                TextSize = TextSize,
                Color = new Color(CurrentTextColor)
            };
            canvas.DrawText(Text,
                            leftSideText,
                            canvas.Height / 2 - (textPaint.Descent() + textPaint.Ascent()) / 2,
                            textPaint);
        }
    }
}
