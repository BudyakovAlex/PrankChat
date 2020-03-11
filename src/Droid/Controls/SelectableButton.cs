using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Controls
{
    public class SelectableButton : Button
    {
        private Drawable _drawableForSelectedState;
        private Drawable _drawableForUnselectedState;
        private string _typeArbitrationButton;

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

            ChangeSelectedState();
        }

        private void ChangeSelectedState()
        {
            if ((_arbitrationValue == ArbitrationValueType.Positive && _typeArbitrationButton == "positive") ||
                (_arbitrationValue == ArbitrationValueType.Negative && _typeArbitrationButton == "negative") ||
                _arbitrationValue == null)
            {
                SetStyle(true, 1f, _drawableForSelectedState);
            }
            else
            {
                SetStyle(false, 0.5f, _drawableForUnselectedState);
            }
        }

        private void SetStyle(bool enabled, float alpha, Drawable leftDrawable)
        {
            Enabled = enabled;
            Alpha = alpha;
            SetCompoundDrawablesRelativeWithIntrinsicBounds(leftDrawable, null, null, null);
        }
    }
}
