using Android.Text;
using Android.Widget;
using Java.Lang;
using System;
using System.Linq;

namespace PrankChat.Mobile.Droid.Listeners
{
    public class TextChangeListener : Java.Lang.Object, ITextWatcher
    {
        private Action<ICharSequence> _action;
        private EditText _editText;

        public TextChangeListener(EditText editText, Action<ICharSequence> action)
        {
            _action = action;
            _editText = editText;
        }

        public void AfterTextChanged(IEditable s)
        {
            
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
            
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            _action?.Invoke(s);
        }
    }
}