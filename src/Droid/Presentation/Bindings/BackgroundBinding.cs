using System;
using System.Reflection;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    internal class BackgroundBinding : MvxPropertyInfoTargetBinding<View>
    {
        public static string TargetBinding = "Background";

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public BackgroundBinding(object target, PropertyInfo targetPropertyInfo) : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (target is View view && value is string drawableName)
            {
                var res = (int)typeof(Resource.Drawable).GetField(drawableName).GetValue(null);
                view.SetBackgroundResource(res);
            }
        }
    }
}
