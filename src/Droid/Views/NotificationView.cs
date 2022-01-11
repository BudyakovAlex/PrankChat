using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Notification;
using PrankChat.Mobile.Core.ViewModels.Notification.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Notifications;
using PrankChat.Mobile.Droid.Views.Base;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Binding.Combiners;

namespace PrankChat.Mobile.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class NotificationView : BaseView<NotificationViewModel>
    {
        private MvxSwipeRefreshLayout _refreshView;
        private EndlessRecyclerView _recyclerView;
        private RecycleViewBindableAdapter _adapter;
        private View _emptyView;

        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Localization.Resources.Notifications;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.notifications_layout);
        }

        protected override void SetViewProperties()
        {
            InitializeEmptyView();
            _refreshView = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh);

            _recyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.recycler_view);
            var layoutManager = new SafeLinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<NotificationItemViewModel, NotificationItemViewHolder>(Resource.Layout.cell_notification);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<NotificationView, NotificationViewModel>();

            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_refreshView).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshView).For(v => v.RefreshCommand).To(vm => vm.ReloadItemsCommand);
            bindingSet.Bind(_recyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_emptyView)
                .For(v => v.BindVisible())
                .ByCombining(new MvxAndValueCombiner(),
                  vm => vm.IsEmpty,
                  vm => vm.IsNotBusy,
                  vm => vm.IsInitialized);
        }

        private void InitializeEmptyView()
        {
            _emptyView = FindViewById<View>(Resource.Id.empty_view);
            var emptyViewTitleTextView = _emptyView.FindViewById<TextView>(Resource.Id.title_text_view);
            emptyViewTitleTextView.Text = Core.Localization.Resources.NotificationsListIsEmpty;
        }
    }
}
