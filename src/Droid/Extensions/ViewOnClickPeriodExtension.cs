using Android.App;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class ViewOnClickPeriodExtension
    {
        public static void SetPeriodActionOnEditText(this EditText editText, Context context, DateTime? dataTime, EventHandler<DatePickerDialog.DateSetEventArgs> eventHandler)
        {
            editText.SetClickActionOnEditText(actionMethod);

            void actionMethod(View view)
            {
                editText.InputType = InputTypes.Null;
                var date = dataTime ?? DateTime.Now;
                var datePickerDialog = new DatePickerDialog(context, eventHandler, date.Year, date.Month, date.Day);
                datePickerDialog.DatePicker.MaxDate = DateTime.Now.ToDialogPickerDate();
                datePickerDialog.Show();
            }
        }
    }
}