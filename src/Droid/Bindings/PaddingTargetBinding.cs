using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Bindings
{
    public class PaddingTargetBinding : MvxAndroidTargetBinding
    {
        public const string StartPadding = nameof(StartPadding);
        public const string EndPadding = nameof(EndPadding);
        public const string TopPadding = nameof(TopPadding);
        public const string BottomPadding = nameof(BottomPadding);

        private readonly string _targetPaddingType;

        public override Type TargetType => typeof(View);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public PaddingTargetBinding(object target, string targetPaddingType) : base(target)
        {
            _targetPaddingType = targetPaddingType;
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (target is View view && value is int padding)
            {
                switch (_targetPaddingType)
                {
                    case StartPadding:
                        view.SetPadding(padding, view.PaddingTop, view.PaddingEnd, view.PaddingBottom);
                        break;
                    case EndPadding:
                        view.SetPadding(view.PaddingStart, view.PaddingTop, padding, view.PaddingBottom);
                        break;
                    case TopPadding:
                        view.SetPadding(view.PaddingStart, padding, view.PaddingEnd, view.PaddingBottom);
                        break;
                    case BottomPadding:
                        view.SetPadding(view.PaddingStart, view.PaddingTop, view.PaddingEnd, padding);
                        break;
                }
            }
        }
    }
}
