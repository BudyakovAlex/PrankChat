using Android.Widget;
using Java.Lang;
using System.Linq;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class EditTextExtensions
    {
        public static void MoveCursorBeforeSymbol(this EditText editText, string lastSymbol, ICharSequence s)
        {
            if (s == null || s.Count() < 1)
            {
                return;
            }

            var text = s.ToString();
            if (text.EndsWith(lastSymbol))
            {
                var penultСharIndex = s.Count() - 2;
                editText.SetSelection(penultСharIndex);
            }
        }
    }
}