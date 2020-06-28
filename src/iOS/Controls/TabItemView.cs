using System;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    public class TabItemView : UIButton
    {
        private const float FontSize = 14f;

        private static readonly UIFont SelectedFont = Theme.Font.MediumOfSize(FontSize);
        private static readonly UIFont UnselectedFont = Theme.Font.RegularFontOfSize(FontSize);

        private bool _isSelected;

        public TabItemView(string text, Action onTap)
        {
            OnTap = onTap;

            Initialize();
            SetTitle(text, UIControlState.Normal);
        }

        public Action OnTap { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                Font = _isSelected ? SelectedFont : UnselectedFont;
            }
        }

        private void Initialize()
        {
            Font = UnselectedFont;
            SetTitleColor(Theme.Color.Text, UIControlState.Normal);
        }
    }
}
