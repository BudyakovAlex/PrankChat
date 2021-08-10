using System;
using MvvmCross.Platforms.Ios;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Dialogs.DatePicker
{
    public partial class DatePickerController : UIViewController
    {
        private DateTime _currentDate;
        private Action<DateTime?> _resultAction;

        public DatePickerController(DateTime currentDate, Action<DateTime?> resultAction) : base("DatePickerController", null)
        {
            _currentDate = currentDate;
            _resultAction = resultAction;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            datePicker.Date = _currentDate.Date.ToNSDate();
            datePicker.BackgroundColor = Theme.Color.White;
            datePicker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;

            doneButton.Title = Resources.Select;
            cancelButton.Clicked += CancelButtonClicked;

            cancelButton.Title = Resources.Cancel;
            doneButton.Clicked += DoneButtonClicked;
        }

        public override void ViewDidDisappear(bool animated)
        {
            cancelButton.Clicked -= CancelButtonClicked;
            doneButton.Clicked -= DoneButtonClicked;
            base.ViewDidDisappear(animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetCommonBackground(true);
        }

        public override void ViewWillDisappear(bool animated)
        {
            SetCommonBackground(false);
            base.ViewWillDisappear(animated);
        }

        private void DoneButtonClicked(object sender, EventArgs e)
        {
            _resultAction?.Invoke(datePicker.Date.ToDateTimeUtc());
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            _resultAction?.Invoke(null);
        }

        private void SetCommonBackground(bool enable)
        {
            UIView.Animate(enable ? 0.4f : 0.1f, 0, UIViewAnimationOptions.AllowAnimatedContent, () => View.BackgroundColor = enable ? Theme.Color.BlackTransparentBackground : UIColor.Clear, null);
        }
    }
}

