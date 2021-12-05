using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace PrankChat.Mobile.iOS.Binding
{
    public class UITabBarControllerIsEnabedTargetBinding : MvxTargetBinding<UITabBarController, bool>
    {
        public UITabBarControllerIsEnabedTargetBinding(UITabBarController target) : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValue(bool value)
        {
            Target.TabBar.UserInteractionEnabled = value;
            foreach (var tabBarItem in Target.TabBar.Items)
            {
                tabBarItem.Enabled = value;
            }
        }
    }
}