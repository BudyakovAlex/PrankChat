using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Commands;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class ViewTouchTargetBinding : MvxAndroidTargetBinding
    {
        public static string PropertyName = "Touch";

        private readonly View view;

        private IMvxCommand command;

        public ViewTouchTargetBinding(View view) : base(view)
        {
            this.view = view;
            this.view.Touch += OnViewTouch;
        }

        public override void SetValue(object value)
        {
            command = value as IMvxCommand;
        }

        public override Type TargetType => typeof(IMvxCommand);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                view.Touch -= OnViewTouch;
            }

            base.Dispose(isDisposing);
        }

        protected override void SetValueImpl(object target, object value)
        {
        }

        private void OnViewTouch(object sender, View.TouchEventArgs eventArgs)
        {
            if (command == null || eventArgs.Event.Action != MotionEventActions.Up)
            {
                eventArgs.Handled = true;
                return;
            }

            eventArgs.Handled = false;
            command.Execute();
        }
    }
}