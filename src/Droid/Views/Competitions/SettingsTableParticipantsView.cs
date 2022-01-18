using Android.App;
using Android.Content.PM;
using PrankChat.Mobile.Core.ViewModels.Competitions;
using PrankChat.Mobile.Droid.Views.Base;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Android.OS;
using Android.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using PrankChat.Mobile.Droid.Adapters;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Competitions;

namespace PrankChat.Mobile.Droid.Views.Competitions
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingsTableParticipantsView : BaseView<SettingsTableParticipantsViewModel>
    {
        private TextView _prizePoolTitleTextView;
        private TextView _fullPrizePoolTextView;
        private TextView _leftToDistributeTextView;
        private TextView _leftToDistributeTitleTextView;
        private MvxRecyclerView _contentRecyclerView;
        private LinearLayout _addPlaceLinearLayout;
        private TextView _warningTextView;
        private Button _applyButton;
        private RecycleViewBindableAdapter _adapter;

        protected override string TitleActionBar => Core.Localization.Resources.ConfiguringTheParticipantTable;

        protected override bool HasBackButton => true;

        public double PrizePool
        {
            set => _fullPrizePoolTextView.Text = $"{value}{Core.Localization.Resources.Currency} / 100{Core.Localization.Resources.Percent}";
        }

        public double LeftToDistribute
        {
            set => _leftToDistributeTextView.Text = $"{value}{Core.Localization.Resources.Percent}";
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState, Resource.Layout.activity_settings_table_participants);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _prizePoolTitleTextView = FindViewById<TextView>(Resource.Id.prize_pool_title_text_view);
            _fullPrizePoolTextView = FindViewById<TextView>(Resource.Id.prize_pool_text_view);
            _leftToDistributeTextView = FindViewById<TextView>(Resource.Id.left_to_distribute_text_view);
            _leftToDistributeTitleTextView = FindViewById<TextView>(Resource.Id.left_to_distribute_title_text_view);
            _contentRecyclerView = FindViewById<MvxRecyclerView>(Resource.Id.content_recycler_view);
            _addPlaceLinearLayout = FindViewById<LinearLayout>(Resource.Id.add_place_linear_layout);
            _warningTextView = FindViewById<TextView>(Resource.Id.warning_text_view);
            _applyButton = FindViewById<Button>(Resource.Id.apply_button);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _contentRecyclerView.Adapter = _adapter;
            _contentRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<PlaceTableParticipantsItemViewModel, PlaceTableParticipantsItemViewHolder>(Resource.Layout.cell_place_table_participant);

            FindViewById<TextView>(Resource.Id.add_place_text_view).Text = Core.Localization.Resources.AddPrizePlace;
            _warningTextView.Text = Core.Localization.Resources.DistributePrizeFundWarningMessage;
            _applyButton.Text = Core.Localization.Resources.Apply;
            _leftToDistributeTitleTextView.Text = $"{Core.Localization.Resources.RemainsToDistribute} - ";
            _prizePoolTitleTextView.Text = $"{Core.Localization.Resources.PrizePool} - ";
        }

        protected override void Bind()
        {
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(this).For(nameof(PrizePool)).To(vm => vm.PrizePool);
            bindingSet.Bind(this).For(nameof(LeftToDistribute)).To(vm => vm.LeftToDistribtePercent);
            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_addPlaceLinearLayout).For(v => v.BindClick()).To(vm => vm.AddPlaceCommand);
            bindingSet.Bind(_warningTextView).For(v => v.BindHidden()).To(vm => vm.IsWarning);
            bindingSet.Bind(_applyButton).For(v => v.BindClick()).To(vm => vm.ApplyCommand);
        }
    }
}