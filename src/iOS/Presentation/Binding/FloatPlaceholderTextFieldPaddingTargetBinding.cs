using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using PrankChat.Mobile.iOS.Controls;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Binding
{
    public class FloatPlaceholderTextFieldPaddingTargetBinding : MvxTargetBinding<FloatPlaceholderTextField, int>
    {
        public const string StartPadding = nameof(StartPadding);
        public const string EndPadding = nameof(EndPadding);
        public const string TopPadding = nameof(TopPadding);
        public const string BottomPadding = nameof(BottomPadding);

        private readonly string _targetPaddingType;

        public FloatPlaceholderTextFieldPaddingTargetBinding(FloatPlaceholderTextField target, string targetPaddingType) : base(target)
        {
            _targetPaddingType = targetPaddingType;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValue(int value)
        {
            switch (_targetPaddingType)
            {
                case StartPadding:
                    Target.EdgeInsets = new UIEdgeInsets(Target.EdgeInsets.Top, value, Target.EdgeInsets.Bottom, Target.EdgeInsets.Right);
                    break;
                case EndPadding:
                    Target.EdgeInsets = new UIEdgeInsets(Target.EdgeInsets.Top, Target.EdgeInsets.Left, Target.EdgeInsets.Bottom, value);
                    break;
                case TopPadding:
                    Target.EdgeInsets = new UIEdgeInsets(value, Target.EdgeInsets.Left, Target.EdgeInsets.Bottom, Target.EdgeInsets.Right);
                    break;
                case BottomPadding:
                    Target.EdgeInsets = new UIEdgeInsets(Target.EdgeInsets.Top, Target.EdgeInsets.Left, value, Target.EdgeInsets.Right);
                    break;
            }
        }
    }
}
