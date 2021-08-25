using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.PercentLayout.Widget;
using FFImageLoading.Cross;
using Google.Android.Material.Button;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
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
        private View _uploadingInfoContainerView;
        private TextView _orderDetailsDescriptionTextView;
        private MaterialButton _takeOrderMaterialButton;
        private MaterialButton _subribeMaterialButton;
        private MaterialButton _unsubcribeMaterialButton;
        private LinearLayout _videoLoadLinearLayout;
        private MaterialButton _loadVideMaterialButton;
        private MaterialButton _executeMaterialButton;
        private MaterialButton _cancelMaterialButton;
        private View _separatorView;
        private TextView _takeTheOrderTextView;
        private CircleCachedImageView _executorPhotoImageView;
        private TextView _executorNameTextView;
        private TextView _takeTheOrderDateTextView;
        private CardView _fullVideoCardView;
        private MvxCachedImageView _completedVideoImageView;
        private ConstraintLayout _processingViewConsraintLayout;
        private PercentRelativeLayout _percentRelativeLayout;
        private SelectableButton _yesSelectableButton;
        private SelectableButton _noSelectableButton;
        private MaterialButton _acceptSelectableButton;
        private MaterialButton _arqueOrderSelectableButton;
        private FrameLayout _animationFrameLayout;
        private MvxSwipeRefreshLayout _mvxSwipeRefreshLayout;
        private CircleCachedImageView _userPhotoImageView;
        private TextView _profileNameTextView;
        private TextView _orderTitleTextView;
        private View _orderDescriptionView;
        private TextView _orderDescriptionTextView;
        private TextView _priceValueTextView;
        private TextView _orderDetailsViewTimeTextView;
        private LinearLayout _timeDaysValueLinearLayout;
        private TextView _timeDaysTextView;
        private TextView _timeHoursTextView;
        private TextView _timeMinutesTextView;

        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Localization.Resources.OrderDetailsView_Title;

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
            _uploadingInfoContainerView = FindViewById<View>(Resource.Id.uploading_info_container);
            _orderDetailsDescriptionTextView = FindViewById<TextView>(Resource.Id.order_description_text_view);
            _uploadingInfoContainerView.SetRoundedCorners(DisplayUtils.DpToPx(15));
            _takeOrderMaterialButton = FindViewById<MaterialButton>(Resource.Id.take_order_button);
            _subribeMaterialButton = FindViewById<MaterialButton>(Resource.Id.subscribe_material_button);
            _unsubcribeMaterialButton = FindViewById<MaterialButton>(Resource.Id.unsubscribe_button);
            _videoLoadLinearLayout = FindViewById<LinearLayout>(Resource.Id.video_load_linear_layout);
            _loadVideMaterialButton = FindViewById<MaterialButton>(Resource.Id.load_video_material_button);
            _executeMaterialButton = FindViewById<MaterialButton>(Resource.Id.execute_button);
            _cancelMaterialButton = FindViewById<MaterialButton>(Resource.Id.cancel_order_details_button);
            _separatorView = FindViewById<View>(Resource.Id.order_details_separator_view);
            _takeTheOrderTextView = FindViewById<TextView>(Resource.Id.take_the_order_text_view);
            _executorPhotoImageView = FindViewById<CircleCachedImageView>(Resource.Id.executor_photo_image_view);
            _executorNameTextView = FindViewById<TextView>(Resource.Id.executor_name_text_view);
            _takeTheOrderDateTextView = FindViewById<TextView>(Resource.Id.take_the_order_date_text_view);
            _fullVideoCardView = FindViewById<CardView>(Resource.Id.full_video_card_view);
            _completedVideoImageView = FindViewById<MvxCachedImageView>(Resource.Id.completed_video_image_view);
            _processingViewConsraintLayout = FindViewById<ConstraintLayout>(Resource.Id.processing_view_constraintLayout);
            _percentRelativeLayout = FindViewById<PercentRelativeLayout>(Resource.Id.video_percent_relative_layout);
            _yesSelectableButton = FindViewById<SelectableButton>(Resource.Id.yes_button);
            _noSelectableButton = FindViewById<SelectableButton>(Resource.Id.no_button);
            _acceptSelectableButton = FindViewById<MaterialButton>(Resource.Id.accept_button);
            _arqueOrderSelectableButton = FindViewById<MaterialButton>(Resource.Id.arque_order_button);
            _animationFrameLayout = FindViewById<FrameLayout>(Resource.Id.animation_frame_layout);
            _mvxSwipeRefreshLayout = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.order_swipe_refresh_layout);
            _userPhotoImageView = FindViewById<CircleCachedImageView>(Resource.Id.user_photo);
            _profileNameTextView = FindViewById<TextView>(Resource.Id.profile_name_text_view);
            _orderTitleTextView = FindViewById<TextView>(Resource.Id.order_title_text_view);
            _orderDescriptionView = FindViewById<View>(Resource.Id.order_description_view);
            _orderDescriptionTextView = FindViewById<TextView>(Resource.Id.order_description_text_view);
            _priceValueTextView = FindViewById<TextView>(Resource.Id.price_value_text_view);
            _orderDetailsViewTimeTextView = FindViewById<TextView>(Resource.Id.order_details_view_time_text_view);
            _timeDaysValueLinearLayout = FindViewById<LinearLayout>(Resource.Id.linear_layout_time_days_value);
            _timeDaysTextView = FindViewById<TextView>(Resource.Id.time_days_text_view);
            _timeHoursTextView = FindViewById<TextView>(Resource.Id.time_hours_text_view);
            _timeMinutesTextView = FindViewById<TextView>(Resource.Id.time_minutes_text_view);

            _uploadingProgressBar.ProgressColor = Color.White;
            _uploadingProgressBar.RingThickness = 5;
            _uploadingProgressBar.BaseColor = Color.Gray;
            _uploadingProgressBar.Progress = 0f;
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<OrderDetailsView, OrderDetailsViewModel>();

            bindingSet.Bind(_uploadingContainerView).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsUploading);
            bindingSet.Bind(_uploadingProgressBar).For(v => v.Progress).To(vm => vm.VideoSectionViewModel.UploadingProgress);
            bindingSet.Bind(_uploadingProgressBar).For(v => v.BindClick()).To(vm => vm.VideoSectionViewModel.CancelUploadingCommand);
            bindingSet.Bind(_uploadedTextView).For(v => v.Text).To(vm => vm.VideoSectionViewModel.UploadingProgressStringPresentation);
            bindingSet.Bind(_orderDetailsDescriptionTextView).For(v => v.TextAlignment).To(vm => vm.IsHiddenOrder)
                      .WithConversion(new BoolToStateConverter<TextAlignment>(TextAlignment.TextStart, TextAlignment.Center));
            bindingSet.Bind(_takeOrderMaterialButton).For(v => v.BindClick()).To(vm => vm.TakeOrderCommand);
            bindingSet.Bind(_takeOrderMaterialButton).For(v => v.BindVisible()).To(vm => vm.IsTakeOrderAvailable);
            bindingSet.Bind(_subribeMaterialButton).For(v => v.BindClick()).To(vm => vm.SubscribeOrderCommand);
            bindingSet.Bind(_subribeMaterialButton).For(v => v.BindVisible()).To(vm => vm.IsSubscribeAvailable);
            bindingSet.Bind(_unsubcribeMaterialButton).For(v => v.BindClick()).To(vm => vm.UnsubscribeOrderCommand);
            bindingSet.Bind(_unsubcribeMaterialButton).For(v => v.BindVisible()).To(vm => vm.IsUnsubscribeAvailable);
            bindingSet.Bind(_videoLoadLinearLayout).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsVideoLoadAvailable);
            bindingSet.Bind(_loadVideMaterialButton).For(v => v.BindClick()).To(vm => vm.VideoSectionViewModel.LoadVideoCommand);
            bindingSet.Bind(_executeMaterialButton).For(v => v.BindClick()).To(vm => vm.ExecuteOrderCommand);
            bindingSet.Bind(_executeMaterialButton).For(v => v.BindVisible()).To(vm => vm.IsExecuteOrderAvailable);
            bindingSet.Bind(_cancelMaterialButton).For(v => v.BindClick()).To(vm => vm.CancelOrderCommand);
            bindingSet.Bind(_cancelMaterialButton).For(v => v.BindVisible()).To(vm => vm.IsCancelOrderAvailable);
            bindingSet.Bind(_separatorView).For(v => v.BindVisible()).To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_takeTheOrderTextView).For(v => v.BindVisible()).To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_executorPhotoImageView).For(v => v.ImagePath).To(vm => vm.ExecutorSectionViewModel.ExecutorPhotoUrl);
            bindingSet.Bind(_executorPhotoImageView).For(v => v.PlaceholderText).To(vm => vm.ExecutorSectionViewModel.ExecutorShortName);
            bindingSet.Bind(_executorPhotoImageView).For(v => v.BindVisible()).To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_executorPhotoImageView).For(v => v.BindClick()).To(vm => vm.ExecutorSectionViewModel.OpenExecutorProfileCommand);
            bindingSet.Bind(_executorNameTextView).For(v => v.Text).To(vm => vm.ExecutorSectionViewModel.ExecutorName);
            bindingSet.Bind(_executorNameTextView).For(v => v.BindVisible()).To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_takeTheOrderDateTextView).For(v => v.Text).To(vm => vm.StartOrderDate);
            bindingSet.Bind(_takeTheOrderDateTextView).For(v => v.BindVisible()).To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_fullVideoCardView).For(v => v.BindClick()).To(vm => vm.VideoSectionViewModel.ShowFullVideoCommand);
            bindingSet.Bind(_fullVideoCardView).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsVideoAvailable);
            bindingSet.Bind(_completedVideoImageView).For(v => v.ImagePath).To(vm => vm.VideoSectionViewModel.VideoPlaceholderUrl);
            bindingSet.Bind(_processingViewConsraintLayout).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsVideoProcessing);
            bindingSet.Bind(_percentRelativeLayout).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsDecideVideoAvailable);
            bindingSet.Bind(_yesSelectableButton).For(v => v.BindClick()).To(vm => vm.YesCommand);
            bindingSet.Bind(_yesSelectableButton).For(v => v.ArbitrationValue).To(vm => vm.SelectedArbitration);
            bindingSet.Bind(_yesSelectableButton).For(v => v.Text).To(vm => vm.YesText);
            bindingSet.Bind(_noSelectableButton).For(v => v.BindClick()).To(vm => vm.NoCommand);
            bindingSet.Bind(_noSelectableButton).For(v => v.ArbitrationValue).To(vm => vm.SelectedArbitration);
            bindingSet.Bind(_noSelectableButton).For(v => v.Text).To(vm => vm.NoText);
            bindingSet.Bind(_acceptSelectableButton).For(v => v.BindClick()).To(vm => vm.AcceptOrderCommand);
            bindingSet.Bind(_acceptSelectableButton).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsDecisionVideoAvailable);
            bindingSet.Bind(_arqueOrderSelectableButton).For(v => v.BindClick()).To(vm => vm.ArqueOrderCommand);
            bindingSet.Bind(_arqueOrderSelectableButton).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsDecisionVideoAvailable);
            bindingSet.Bind(_animationFrameLayout).For(v => v.BindVisible()).To(vm => vm.VideoSectionViewModel.IsBusy);
            bindingSet.Bind(_mvxSwipeRefreshLayout).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_mvxSwipeRefreshLayout).For(v => v.RefreshCommand).To(vm => vm.LoadOrderDetailsCommand);
            bindingSet.Bind(_userPhotoImageView).For(v => v.ImagePath).To(vm => vm.CustomerSectionViewModel.ProfilePhotoUrl).OneWay();
            bindingSet.Bind(_userPhotoImageView).For(v => v.PlaceholderText).To(vm => vm.CustomerSectionViewModel.ProfileShortName).OneWay();
            bindingSet.Bind(_userPhotoImageView).For(v => v.BindClick()).To(vm => vm.CustomerSectionViewModel.OpenCustomerProfileCommand);
            bindingSet.Bind(_profileNameTextView).For(v => v.Text).To(vm => vm.CustomerSectionViewModel.ProfileName);
            bindingSet.Bind(_orderTitleTextView).For(v => v.Text).To(vm => vm.OrderTitle);
            bindingSet.Bind(_orderDescriptionView).For(v => v.BindVisible()).To(vm => vm.IsHiddenOrder);
            bindingSet.Bind(_orderDescriptionTextView).For(v => v.Text).To(vm => vm.OrderDescription);
            bindingSet.Bind(_priceValueTextView).For(v => v.Text).To(vm => vm.PriceValue);
            bindingSet.Bind(_orderDetailsViewTimeTextView).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(_timeDaysValueLinearLayout).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(_timeDaysTextView).For(v => v.Text).To(vm => vm.TimeDaysValue);
            bindingSet.Bind(_timeHoursTextView).For(v => v.Text).To(vm => vm.TimeHourValue);
            bindingSet.Bind(_timeMinutesTextView).For(v => v.Text).To(vm => vm.TimeMinutesValue);
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
