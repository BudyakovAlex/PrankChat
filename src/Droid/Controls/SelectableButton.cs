using System;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.Design.Button;
using Android.Support.V4.Content;
using Android.Util;
using Android.Widget;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Controls
{
    public class SelectableButton : MaterialButton
    {
        private Drawable _drawableForSelectedState;
        private Drawable _drawableForUnselectedState;
        private string _typeArbitrationButton;
        private GradientDrawable backgroundDrawable;

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

        public float Transparency { get; set; } = 0.5f;

        #region Constructions

        protected SelectableButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public SelectableButton(Context context) : base(context)
        {
            Initialize(context);
        }

        public SelectableButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context, attrs);
        }

        public SelectableButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(context, attrs);
        }

        #endregion

        private void Initialize(Context context, IAttributeSet attrs = null)
        {
            if (attrs != null)
            {
                var array = context.ObtainStyledAttributes(attrs, Resource.Styleable.SelectableButton, 0, 0);

                _drawableForSelectedState = array.GetDrawable(Resource.Styleable.SelectableButton_selected_drawable);
                _drawableForUnselectedState = array.GetDrawable(Resource.Styleable.SelectableButton_unselected_drawable);
                _typeArbitrationButton = array.GetNonResourceString(Resource.Styleable.SelectableButton_type_arbitration);

                array.Recycle();
            }

            SetAllCaps(false);

            backgroundDrawable = new GradientDrawable();
            backgroundDrawable.SetShape(ShapeType.Rectangle);
            backgroundDrawable.SetStroke(3, GetColorStateList(Resource.Color.applicationTransparent));
            backgroundDrawable.SetCornerRadius(15);

            IconGravity = IconGravityTextStart;
            IconTint = null;
            IconPadding = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 20f, Resources.DisplayMetrics);
            BackgroundTintList = null;
            TextAlignment = Android.Views.TextAlignment.Center;

            ChangeSelectedState();
        }

        private void ChangeSelectedState()
        {
            if ((_arbitrationValue == ArbitrationValueType.Positive && _typeArbitrationButton == "positive") ||
                (_arbitrationValue == ArbitrationValueType.Negative && _typeArbitrationButton == "negative") ||
                _arbitrationValue == null)
            {
                SetPositiveStyle();
            }
            else
            {
                SetNegativeStyle();
            }
        }

        private void SetPositiveStyle()
        {
            SetStyle(1f, Resource.Color.applicationWhite, _drawableForSelectedState, Resource.Color.accent);
        }

        private void SetNegativeStyle()
        {
            SetStyle(Transparency, Resource.Color.accent, _drawableForUnselectedState, Resource.Color.applicationWhite);
        }

        private void SetStyle(float alpha, int textColor, Drawable leftDrawable, int backgroundColor)
        {
            Alpha = alpha;
            SetTextColor(GetColorStateList(textColor));
            backgroundDrawable.SetColor(GetColorStateList(backgroundColor));
            Icon = leftDrawable;
            Background = backgroundDrawable;
        }

        private Android.Content.Res.ColorStateList GetColorStateList(int id) => ContextCompat.GetColorStateList(Application.Context, id);
    }
}
