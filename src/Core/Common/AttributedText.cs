using System.Drawing;

namespace PrankChat.Mobile.Core.Common
{
    public sealed class AttributedText
    {
        public AttributedText(string text,
                              int? textSize = null,
                              Color? textColor = null,
                              Color? backgroundColor = null)
        {
            Text = text;
            TextSize = textSize;
            TextColor = textColor;
            BackgroundColor = backgroundColor;
        }

        public string Text { get; }

        public int? TextSize { get; }

        public Color? TextColor { get; }

        public Color? BackgroundColor { get; }
    }
}
