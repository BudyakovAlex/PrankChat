using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    public partial class PlaceTableParticipantsItemCell : BaseTableCell<PlaceTableParticipantsItemCell, PlaceTableParticipantsItemViewModel>
    {
        private UITextPosition _previousPosition;

        protected PlaceTableParticipantsItemCell(IntPtr handle)
            : base(handle)
        {
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            PercentTextField.SetDarkStyle(rightPadding: 14f);

            PercentTextField.TextAlignment = UITextAlignment.Right;
        }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<PlaceTableParticipantsItemCell, PlaceTableParticipantsItemViewModel>();

            bindingSet.Bind(PercentTextField).For(v => v.Text).To(vm => vm.Percent)
                .WithConversion<PercentConverter>();
            bindingSet.Bind(PercentTextField).For(v => v.Placeholder).To(vm => vm.Title);
        }

        protected override void Subscribe()
        {
            base.Subscribe();

            PercentTextField.EditingChanged += OnRightTextAlignmentTextFieldEditingChanged;
        }

        private void OnRightTextAlignmentTextFieldEditingChanged(object sender, EventArgs e)
        {
            if (!(sender is UITextField textField))
            {
                return;
            }

            var text = textField.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if (!text.EndsWith(Resources.Percent))
            {
                return;
            }

            var position = textField.GetPosition(textField.EndOfDocument, -2);
            if (_previousPosition == position)
            {
                return;
            }

            _previousPosition = position;
            textField.SelectedTextRange = textField.GetTextRange(position, position);
        }
    }
}