using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Binding
{
    public class UIButtonSelectedTargetBinding : MvxTargetBinding<UIButton, bool>
    {
        public UIButtonSelectedTargetBinding(UIButton target) : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected override void SetValue(bool value)
        {
            Target.Selected = (bool)value;
        }
    }
}
