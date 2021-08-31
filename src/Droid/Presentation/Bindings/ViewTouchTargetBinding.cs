using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Commands;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class ViewTouchTargetBinding : MvxAndroidTargetBinding<View, IMvxCommand>
    {
        private IMvxCommand _command;
        private bool _isDisposed;

        public ViewTouchTargetBinding(View view) : base(view)
        {
            Target.Touch += OnViewTouch;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing && !_isDisposed)
            {
                Target.Touch -= OnViewTouch;
            }

            base.Dispose(isDisposing);
            _isDisposed = true;
        }

        protected override void SetValueImpl(View target, IMvxCommand value)
        {
            _command = value;
        }

        private void OnViewTouch(object sender, View.TouchEventArgs eventArgs)
        {
            if (_command == null || eventArgs.Event.Action != MotionEventActions.Up)
            {
                eventArgs.Handled = true;
                return;
            }

            eventArgs.Handled = false;
            _command.Execute();
        }
    }
}