using Android.Text;
using Java.Lang;
using System;

namespace PrankChat.Mobile.Droid.Listeners
{
    public class TextChangeListener : Java.Lang.Object, ITextWatcher
    {
        private readonly Action<ICharSequence> _action;

        public TextChangeListener(Action<ICharSequence> action)
        {
            _action = action;
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