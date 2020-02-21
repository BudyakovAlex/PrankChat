using System;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Binding
{
    public class UIButtonSelectedTargetBinding : MvxPropertyInfoTargetBinding<UIButton>
    {
        public static string TargetBinding = "Selected";

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public UIButtonSelectedTargetBinding(object target, PropertyInfo targetPropertyInfo) : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            View.Selected = (bool)value;
        }
    }
}
