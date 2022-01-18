using Foundation;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Competitions;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class SettingsTableParticipantsView : BaseViewController<SettingsTableParticipantsViewModel>
    {
        private TableViewSource _tableViewSource;

        public int PrizePool
        {
            set
            {
                var normalTextAttributes = new NSMutableDictionary<NSString, UIFont>
                {
                    { UIStringAttributeKey.Font, Theme.Font.RegularFontOfSize(14) }
                };

                var attributedString = new NSMutableAttributedString($"{Resources.PrizePool} - ", normalTextAttributes);
                var boldTextAttributes = new NSMutableDictionary<NSString, UIFont>()
                {
                    { UIStringAttributeKey.Font, Theme.Font.BoldOfSize(14) }
                };

                var boldString = new NSMutableAttributedString($"{value}{Resources.Currency} / 100%", boldTextAttributes);
                attributedString.Append(boldString);
                FullPrizePoolLabel.AttributedText = attributedString;
            }
        }

        public int LeftToDistribute
        {
            set
            {
                var normalTextAttributes = new NSMutableDictionary<NSString, UIFont>
                {
                    { UIStringAttributeKey.Font, Theme.Font.RegularFontOfSize(14) }
                };
                var attributedString = new NSMutableAttributedString($"{Resources.RemainsToDistribute} - ", normalTextAttributes);
                var boldTextAttributes = new NSMutableDictionary<NSString, NSObject>()
                {
                    { UIStringAttributeKey.Font, Theme.Font.BoldOfSize(14) },
                    { UIStringAttributeKey.ForegroundColor, value == 0 ? UIColor.Black : Theme.Color.WarningColor }
                };
                var boldString = new NSMutableAttributedString($"{value}%", boldTextAttributes);
                attributedString.Append(boldString);
                LeftToDistributeLabel.AttributedText = attributedString;
            }
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            _tableViewSource = new TableViewSource(ItemsTableView)
                 .Register<PlaceTableParticipantsItemViewModel>(PlaceTableParticipantsItemCell.Nib, PlaceTableParticipantsItemCell.CellId);
            ItemsTableView.RowHeight = 58f;
            ItemsTableView.Source = _tableViewSource;
            ItemsTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;


            AddPlaceImageView.Image = UIImage.FromBundle(ImageNames.AddPlaceParticipant);
            ApplyButton.SetDarkStyle(Resources.Apply);
            Title = Resources.ConfiguringTheParticipantTable;
            WarningLabel.SetSmallSubtitleStyle(Resources.DistributePrizeFundWarningMessage);
            AddPlaceLabel.SetTitleStyle(Resources.AddPrizePlace);
            AddPlaceLabel.TextColor = Theme.Color.NewAccentColor;
            WarningLabel.TextColor = Theme.Color.WarningColor;
        }

        protected override void Bind()
        {
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(AddPrizePlaceView).For(v => v.BindTap()).To(vm => vm.AddPlaceCommand);
            bindingSet.Bind(_tableViewSource).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(ApplyButton).For(v => v.BindTouchUpInside()).To(vm => vm.ApplyCommand);
            bindingSet.Bind(this).For(nameof(PrizePool)).To(vm => vm.PrizePool);
            bindingSet.Bind(this).For(nameof(LeftToDistribute)).To(vm => vm.LeftToDistribtePercent);
            bindingSet.Bind(WarningLabel).For(v => v.BindHidden()).To(vm => vm.IsWarning);
        }
    }
}