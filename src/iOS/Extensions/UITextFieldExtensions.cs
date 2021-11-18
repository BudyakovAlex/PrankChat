using System;
using Foundation;
using PrankChat.Mobile.iOS.Controls;
using UIKit;

namespace PrankChat.Mobile.iOS.Extensions
{
    public static class UITextFieldExtensions
    {
        public static UIDatePicker SetDatePickerInputView(this UITextField textField, string dateFormat, UIView parent = null, Action endAction = null, Func<DateTime?> minimumDateFunc = null)
        {
            var datePicker = new UIDatePicker() { Mode = UIDatePickerMode.Date, PreferredDatePickerStyle = UIDatePickerStyle.Wheels };
            var formatter = new NSDateFormatter() { DateFormat = dateFormat };
            var doneButtonTapAction = new Action(() =>
            {
                DatePickerValueChanged(datePicker, EventArgs.Empty);
                datePicker.ValueChanged -= DatePickerValueChanged;
                endAction?.Invoke();
            });
            textField.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                datePicker.ValueChanged += DatePickerValueChanged;
                datePicker.MinimumDate = minimumDateFunc?.Invoke().ToNSDate();
            })
            {
                CancelsTouchesInView = false,
                // Delegate = 
            });

            textField.InputAccessoryView = new DoneAccessoryView(parent ?? textField.Superview, doneButtonTapAction);
            textField.InputView = datePicker;

            return datePicker;

            void DatePickerValueChanged(object sender, EventArgs e)
            {
                textField.Text = formatter.StringFor(datePicker.Date);
            }
        }
    }

    public class UIGestureRecognizerDelegate : NSObject, IUIGestureRecognizerDelegate
    {
    }
}
