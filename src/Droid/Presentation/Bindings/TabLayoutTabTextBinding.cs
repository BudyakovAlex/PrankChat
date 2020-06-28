using Android.Support.Design.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class TabLayoutTabTextBinding : MvxTargetBinding<TabLayout.Tab, string>
    {
        public static string TargetBinding = nameof(TabLayoutTabTextBinding);

        public TabLayoutTabTextBinding(TabLayout.Tab target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValue(string value) => Target.SetText(value);
    }
}
