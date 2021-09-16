using Android.Widget;
using Java.Lang;
using PrankChat.Mobile.Core.Localization;
using System.Linq;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class SetSelectionOnPenultCharExtensions
    {
        public static void SetSelectionOnPenultСhar(this EditText editText)
        {
            editText.SetActionOnEditText(actionMethod);

            void actionMethod(EditText editText, ICharSequence s)
            {
                if (s == null || s.Count() < 1)
                {
                    return;
                }

                var text = s.ToString();
                if (text.EndsWith(Resources.Percent) || text.EndsWith(Resources.Currency))
                {
                    var penultСharIndex = s.Count() - 2;
                    editText.SetSelection(penultСharIndex);
                }
            }
        }
    }
}