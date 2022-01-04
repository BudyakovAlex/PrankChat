using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.Droid.Converters;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Competitions
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class CompetitonStatitsticsView : BaseView<CompetitonStatitsticsViewModel>
    {
        private FrameLayout _loadingOverlay;
        private TextView _profitValueTextView;
        private TextView _participantsValueTextView;
        private TextView _contributionValueTextView;
        private TextView _percentValueTextView;

        protected override string TitleActionBar => "Статистика";

        protected override bool HasBackButton => true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState, Resource.Layout.activity_competion_statistics);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _loadingOverlay = FindViewById<FrameLayout>(Resource.Id.loading_overlay);
            _profitValueTextView = FindViewById<TextView>(Resource.Id.profit_value_text_view);
            _participantsValueTextView = FindViewById<TextView>(Resource.Id.participants_value_text_view);
            _contributionValueTextView = FindViewById<TextView>(Resource.Id.contribution_value_text_view);
            _percentValueTextView = FindViewById<TextView>(Resource.Id.percent_value_text_view);

            FindViewById<TextView>(Resource.Id.profit_title_text_view).Text = "Прибыль";
            FindViewById<TextView>(Resource.Id.participants_title_text_view).Text = "участников";
            FindViewById<TextView>(Resource.Id.contribution_title_text_view).Text = "взнос";
            FindViewById<TextView>(Resource.Id.percent_title_text_view).Text = "% от взноса";
        }

        protected override void Bind()
        {
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_profitValueTextView).For(v => v.Text).To(vm => vm.Profit);
            bindingSet.Bind(_participantsValueTextView).For(v => v.Text).To(vm => vm.Participants);
            bindingSet.Bind(_contributionValueTextView).For(v => v.Text).To(vm => vm.Contribution);
            bindingSet.Bind(_percentValueTextView).For(v => v.Text).To(vm => vm.PercentageFromContribution);
            bindingSet.Bind(_loadingOverlay).For(v => v.Visibility).To(vm => vm.IsBusy)
                      .WithConversion<BoolToGoneConverter>();
        }
    }
}
