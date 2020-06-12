using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Order
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class OrderDetailsView : BaseView<OrderDetailsViewModel>
    {
        private View _uploadingContainerView;
        private CircleProgressBar _uploadingProgressBar;

        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.OrderDetailsView_Title;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_order_details_view);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _uploadingContainerView = FindViewById<View>(Resource.Id.uploading_progress_container);
            _uploadingProgressBar = FindViewById<CircleProgressBar>(Resource.Id.uploading_progress_bar);

            _uploadingProgressBar.ProgressColor = Color.White;
            _uploadingProgressBar.RingThickness = 5;
            _uploadingProgressBar.BaseColor = Color.Gray;
            _uploadingProgressBar.Progress = 0f;
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<OrderDetailsView, OrderDetailsViewModel>();

            bindingSet.Bind(_uploadingContainerView)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsUploading);

            bindingSet.Bind(_uploadingProgressBar)
                      .For(v => v.Progress)
                      .To(vm => vm.UploadingProgress);

            bindingSet.Bind(_uploadingProgressBar)
                      .For(v => v.BindClick())
                      .To(vm => vm.CancelUploadingCommand);

            bindingSet.Apply();
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.order_details_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.settings_menu_item:
                    ViewModel.OpenSettingsCommand.ExecuteAsync();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
