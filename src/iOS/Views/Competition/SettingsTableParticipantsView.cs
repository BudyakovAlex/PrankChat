using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.Base;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class SettingsTableParticipantsView : BaseViewController<SettingsTableParticipantsViewModel>
    {
        private TableViewSource _tableViewSource;

        protected override void SetupControls()
        {
            base.SetupControls();

            _tableViewSource = new TableViewSource(ItemsTableView)
                 .Register<PlaceTableParticipantsItemViewModel>(PlaceTableParticipantsItemCell.Nib, PlaceTableParticipantsItemCell.CellId);
            ItemsTableView.RowHeight = 42f;

            // TODO: Move to AppStrings.
            Title = "Настройка таблицы участников";
        }

        protected override void Bind()
        {
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(AddPrizePlaceView).For(v => v.BindTap()).To(vm => vm.AddPlaceCommand);

        }
    }
}