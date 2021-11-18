using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    public class DoneAccessoryView : UIToolbar
    {
        private readonly UIView _parentView;
        private readonly Action _tapAction;

        private UIButton _doneButton;

        public DoneAccessoryView(UIView parentView, Action tapAction)
        {
            InitView();
            _parentView = parentView;
            _tapAction = tapAction;
        }

        public bool CanCancelEditing { get; set; } = true;

        private void InitView()
        {
            BarTintColor = AppTheme.Theme.Color.Accent;

            TranslatesAutoresizingMaskIntoConstraints = false;
            _doneButton = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
            _doneButton.Font = AppTheme.Theme.Font.RegularFontOfSize(17);
            _doneButton.SetTitle("Готово", UIControlState.Normal);
            _doneButton.SetTitleColor(UIColor.White, UIControlState.Normal);

            var flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            SetItems(new[] { flexibleSpace, new UIBarButtonItem(_doneButton) }, true);

            _doneButton.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                _tapAction?.Invoke();
                _parentView.EndEditing(true);
            }));
        }
    }
}
