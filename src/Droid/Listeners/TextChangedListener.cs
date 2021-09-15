using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Google.Android.Material.TextField;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrankChat.Mobile.Droid.Listeners
{
    public static class TextChangedListenerExtensions
    {
        public static void SetOnTextChangedListener(this EditText textInput) =>
            textInput.AddTextChangedListener(new TextChangedListener(textInput));
    }

    public class TextChangedListener : Java.Lang.Object, ITextWatcher
    {
        private EditText _editText;

        public TextChangedListener(EditText textInputEditText)
        {
            _editText = textInputEditText;
        }

        public void AfterTextChanged(IEditable s)
        {
            
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
            
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (s == null || s.Count() < 1)
            {
                return;
            }
            var text = s.ToString();
            if (text.EndsWith("%") || text.EndsWith(Core.Localization.Resources.Currency))
            {
                _editText.SetSelection(s.Count() - 2);
            }
        }
    }
}