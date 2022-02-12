using Android.Text;
using Android.Text.Style;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Droid.Extensions;
using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Droid.Bindings
{
    public sealed class TextViewAttributedTextTargetBinding : MvxTargetBinding
    {
        public TextViewAttributedTextTargetBinding(TextView target)
            : base(target)
        {
        }

        public override Type TargetType => typeof(TextView);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        private TextView TextView => (TextView)Target;

        public override void SetValue(object value)
        {
            switch (value)
            {
                case null:
                case IList<AttributedText> { Count: 0 }:
                    TextView.Text = string.Empty;
                    break;

                case IList<AttributedText> { Count: 1 } attributedTexts:
                    var firstAttributedText = attributedTexts[0];
                    SetAttributedText(firstAttributedText);
                    break;

                case IList<AttributedText> attributedTexts:
                    var spannableStringBuilder = new SpannableStringBuilder();
                    foreach (var attributedText in attributedTexts)
                    {
                        var spannableString = CreateSpannableString(attributedText);
                        spannableStringBuilder.Append(spannableString);
                    }

                    TextView.SetText(spannableStringBuilder, TextView.BufferType.Spannable);
                    break;

                case AttributedText attributedText:
                    SetAttributedText(attributedText);
                    break;

                default:
                    throw new ArgumentException($"TextViewAttributedTextTargetBinding. Unknown value type: {value.GetType().Name}.");
            }
        }

        private void SetAttributedText(AttributedText attributedText)
        {
            var spannableString = CreateSpannableString(attributedText);
            TextView.SetText(spannableString, TextView.BufferType.Spannable);
        }

        private SpannableString CreateSpannableString(AttributedText attributedText)
        {
            var spannableString = new SpannableString(attributedText.Text);

            if (attributedText.TextSize.HasValue)
            {
                var sizeSpan = new AbsoluteSizeSpan(attributedText.TextSize.Value, true);
                spannableString.SetSpan(sizeSpan, 0, attributedText.Text.Length, SpanTypes.ExclusiveExclusive);
            }

            if (attributedText.TextColor.HasValue)
            {
                var foregroundColor = attributedText.TextColor.Value.ToAndroidColor();
                var foregroundColorSpan = new ForegroundColorSpan(foregroundColor);
                spannableString.SetSpan(foregroundColorSpan, 0, attributedText.Text.Length, SpanTypes.ExclusiveExclusive);
            }

            if (attributedText.BackgroundColor.HasValue)
            {
                var backgroundColor = attributedText.BackgroundColor.Value.ToAndroidColor();
                var backgroundColorSpan = new BackgroundColorSpan(backgroundColor);
                spannableString.SetSpan(backgroundColorSpan, 0, attributedText.Text.Length, SpanTypes.ExclusiveExclusive);
            }

            return spannableString;
        }
    }
}
