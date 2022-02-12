using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.iOS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace PrankChat.Mobile.iOS.Binding
{
    public sealed class UILabelAttributedTextTargetBinding : MvxTargetBinding
    {
        public UILabelAttributedTextTargetBinding(UILabel target)
            : base(target)
        {
            Target = target;
        }

        public override Type TargetType => typeof(UILabel);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected new UILabel Target { get; }

        public override void SetValue(object value)
        {
            switch (value)
            {
                case null:
                case IList<AttributedText> { Count: 0 } _:
                    Target.Text = string.Empty;
                    break;

                case IList<AttributedText> { Count: 1 } list:
                    SetAttributedText(list[0]);
                    break;

                case IList<AttributedText> list:
                    var attributedString = list
                        .Select(CreateAttributedString)
                        .Aggregate((attr1, attr2) =>
                        {
                            attr1.Append(attr2);
                            return attr1;
                        });

                    Target.AttributedText = attributedString;
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
            var attributedString = CreateAttributedString(attributedText);
            Target.AttributedText = attributedString;
        }

        private NSMutableAttributedString CreateAttributedString(AttributedText attributedText)
        {
            var font = attributedText.TextSize.HasValue
                ? Target.Font.WithSize(attributedText.TextSize.Value)
                : Target.Font;

            var textColor = attributedText.TextColor?.ToUIColor();

            return new NSMutableAttributedString(attributedText.Text, font, textColor);
        }
    }
}
