using Android.Text;
using Android.Widget;
using Java.Lang;
using System;
using System.Linq;

namespace PrankChat.Mobile.Droid.Listeners
{
    public class TextChangeListener<T> : Java.Lang.Object, ITextWatcher
    {
        private Action<T, ICharSequence> _action;
        private T _editText;

        public TextChangeListener(Action<T, ICharSequence> action , T editText)
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
            _action.Invoke(_editText, s);
        }
    }
}