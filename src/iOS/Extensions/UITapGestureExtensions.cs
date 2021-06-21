using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace PrankChat.Mobile.iOS.Extensions
{
    public static class UITapGestureExtensions
    {
        public static bool DidTapAttributedTextInLabel(this UITapGestureRecognizer tap, UILabel label, NSRange targetRange)
        {
            var layoutManager = new NSLayoutManager();
            var textContainer = new NSTextContainer(CGSize.Empty);
            var textStorage = new NSTextStorage();
            textStorage.SetString(label.AttributedText);

            layoutManager.AddTextContainer(textContainer);
            textStorage.AddLayoutManager(layoutManager);

            textContainer.LineFragmentPadding = 0;
            textContainer.LineBreakMode = label.LineBreakMode;
            textContainer.MaximumNumberOfLines = (nuint)label.Lines;
            var labelSize = label.Bounds.Size;
            textContainer.Size = labelSize;

            var locationOfTouchInLabel = tap.LocationInView(label);
            var textBoundingBox = layoutManager.GetUsedRectForTextContainer(textContainer);
            var textContainerOffset = new CGPoint((labelSize.Width - textBoundingBox.Size.Width) * 0.5 - textBoundingBox.Location.X,
            (labelSize.Height - textBoundingBox.Size.Height) * 0.5 - textBoundingBox.Location.Y);

            var locationOfTouchInTextContainer =
                new CGPoint(
                    locationOfTouchInLabel.X - textContainerOffset.X,
                    locationOfTouchInLabel.Y - textContainerOffset.Y);

            var indexOfCharacter = layoutManager.GetCharacterIndex(locationOfTouchInTextContainer, textContainer, out nfloat partialFraction);
            if (((nint)indexOfCharacter >= targetRange.Location) && ((nint)indexOfCharacter < targetRange.Location + targetRange.Length))
            {
                return true;
            }

            return false;
        }
    }
}
