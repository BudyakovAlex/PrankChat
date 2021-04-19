﻿using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using PrankChat.Mobile.Droid.Utils.Helpers;

namespace PrankChat.Mobile.Droid.Presentation.Views.Order
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class OrderDetailsView : BaseView<OrderDetailsViewModel>
    {
        private View _uploadingContainerView;
        private CircleProgressBar _uploadingProgressBar;
        private TextView _uploadedTextView;
        private View _uploadingInfoContainer;
        private TextView _orderDescriptionTextView;

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
            _uploadedTextView = FindViewById<TextView>(Resource.Id.uploaded_text_view);
            _uploadingInfoContainer = FindViewById<View>(Resource.Id.uploading_info_container);
            _orderDescriptionTextView = FindViewById<TextView>(Resource.Id.order_descroption_text_view);
            _uploadingInfoContainer.SetRoundedCorners(DisplayUtils.DpToPx(30) / 2);

            _uploadingProgressBar.ProgressColor = Color.White;
            _uploadingProgressBar.RingThickness = 5;
            _uploadingProgressBar.BaseColor = Color.Gray;
            _uploadingProgressBar.Progress = 0f;
        }

        protected override void Bind()
        {
            base.Bind();

            var bindingSet = this.CreateBindingSet<OrderDetailsView, OrderDetailsViewModel>();

            bindingSet.Bind(_uploadingContainerView)
                      .For(v => v.BindVisible())
                      .To(vm => vm.VideoSectionViewModel.IsUploading);

            bindingSet.Bind(_uploadingProgressBar)
                      .For(v => v.Progress)
                      .To(vm => vm.VideoSectionViewModel.UploadingProgress);

            bindingSet.Bind(_uploadingProgressBar)
                      .For(v => v.BindClick())
                      .To(vm => vm.VideoSectionViewModel.CancelUploadingCommand);

            bindingSet.Bind(_uploadedTextView)
                      .For(v => v.Text)
                      .To(vm => vm.VideoSectionViewModel.UploadingProgressStringPresentation);

            bindingSet.Bind(_orderDescriptionTextView)
                      .For(v => v.TextAlignment)
                      .To(vm => vm.IsHiddenOrder)
                      .WithConversion(new BoolToStateConverter<TextAlignment>(TextAlignment.TextStart, TextAlignment.Center));

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
