using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Commands;
using MvvmCross.Platforms.Android.Binding.Target;

namespace PrankChat.Mobile.Droid.Presentation.Bindings
{
    public class ViewTouchTargetBinding : MvxAndroidTargetBinding
    {
        public static string TargetBinding = "Touch";

        private readonly View _view;

        private IMvxCommand _command;
        private bool _isDisposed;

        public ViewTouchTargetBinding(View view) : base(view)
        {
            _view = view;
            _view.Touch += OnViewTouch;
        }

        public override void SetValue(object value)
        {
            _command = value as IMvxCommand;
        }

        public override Type TargetType => typeof(IMvxCommand);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing && !_isDisposed)
            {
                _view.Touch -= OnViewTouch;
            }

            base.Dispose(isDisposing);
            _isDisposed = true;
        }

        protected override void SetValueImpl(object target, object value)
        {
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