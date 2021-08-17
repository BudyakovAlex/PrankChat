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
        private View _uploadingInfoContainer;
        private TextView _orderDescriptionTextView;

        //sdfsdfsdf
        private MaterialButton _materialButtonTakeOrder;
        private MaterialButton _materialButtonSubribe;
        private MaterialButton _materialButtonUnsubcribe;
        private LinearLayout _linearLayoutVideoLoad;
        private MaterialButton _materialButtonLoadVideo;
        private MaterialButton _materialButtonExecute;
        private MaterialButton _materialButtonCancel;
        private View _separator;
        private TextView _textViewTakeTheOrderText;
        private CircleCachedImageView _executorPhoto;
        private TextView _textViewExecutorName;
        private TextView _textViewTakeTheOrderDate;
        private CardView _cardViewFullVideo;
        private MvxCachedImageView _completedVideo;
        private ConstraintLayout _processingView;
        private PercentRelativeLayout _percentRelativeLayoutVideo;
        private SelectableButton _selectableButtonYes;
        private SelectableButton _selectableButtonNo;
        private MaterialButton _materialButtonAccept;
        private MaterialButton _materialButtonArqueOrder;
        private FrameLayout _frameLayoutAnimation;
        private MvxSwipeRefreshLayout _mvxSwipeRefreshLayout;
        private CircleCachedImageView _userPhoto;
        private TextView _textViewProfileName;
        private TextView _textViewOrderTitle;
        private View _viewOrderDescription;
        private TextView _textViewOrderDescription;
        private TextView _textViewPriceValue;
        private TextView _textViewoOrderDetailsViewTime;
        private LinearLayout _linearLayoutTimeDaysValue;
        private TextView _textViewTimeDays;
        private TextView _textViewTimeHours;
        private TextView _textViewTimeMinutes;

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
            _uploadingInfoContainer = FindViewById<View>(Resource.Id.uploading_info_container);
            _orderDescriptionTextView = FindViewById<TextView>(Resource.Id.order_descroption_text_view);
            _uploadingInfoContainer.SetRoundedCorners(DisplayUtils.DpToPx(30) / 2);
            //sdfdsffsd
            _materialButtonTakeOrder = FindViewById<MaterialButton>(Resource.Id.take_order_button);
            _materialButtonSubribe = FindViewById<MaterialButton>(Resource.Id.subscribe_material_button);
            _materialButtonUnsubcribe = FindViewById<MaterialButton>(Resource.Id.unsubscribe_button);
            _linearLayoutVideoLoad = FindViewById<LinearLayout>(Resource.Id.linear_layout_video_load);
            _materialButtonLoadVideo = FindViewById<MaterialButton>(Resource.Id.load_video_material_button);
            _materialButtonExecute = FindViewById<MaterialButton>(Resource.Id.execute_button);
            _materialButtonCancel = FindViewById<MaterialButton>(Resource.Id.cancel_order_details_button);
            _separator = FindViewById<View>(Resource.Id.separator);
            _textViewTakeTheOrderText = FindViewById<TextView>(Resource.Id.take_the_order_text);
            _executorPhoto = FindViewById<CircleCachedImageView>(Resource.Id.executor_photo);
            _textViewExecutorName = FindViewById<TextView>(Resource.Id.executor_name);
            _textViewTakeTheOrderDate = FindViewById<TextView>(Resource.Id.take_the_order_date);
            _cardViewFullVideo = FindViewById<CardView>(Resource.Id.card_view_full_video);
            _completedVideo = FindViewById<MvxCachedImageView>(Resource.Id.completed_video);
            _processingView = FindViewById<ConstraintLayout>(Resource.Id.processing_view);
            _percentRelativeLayoutVideo = FindViewById<PercentRelativeLayout>(Resource.Id.percentRelativeLayout_video);
            _selectableButtonYes = FindViewById<SelectableButton>(Resource.Id.yes_button);
            _selectableButtonNo = FindViewById<SelectableButton>(Resource.Id.no_button);
            _materialButtonAccept = FindViewById<MaterialButton>(Resource.Id.accept_button);
            _materialButtonArqueOrder = FindViewById<MaterialButton>(Resource.Id.arque_order_button);
            _frameLayoutAnimation = FindViewById<FrameLayout>(Resource.Id.frame_layout_animation);
            _mvxSwipeRefreshLayout = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.mvxSwipeRefreshLayout);
            _userPhoto = FindViewById<CircleCachedImageView>(Resource.Id.user_photo);
            _textViewProfileName = FindViewById<TextView>(Resource.Id.profile_name);
            _textViewOrderTitle = FindViewById<TextView>(Resource.Id.order_title);
            _viewOrderDescription = FindViewById<View>(Resource.Id.view_order_description);
            _textViewOrderDescription = FindViewById<TextView>(Resource.Id.order_descroption_text_view);
            _textViewPriceValue = FindViewById<TextView>(Resource.Id.price_value);
            _textViewoOrderDetailsViewTime = FindViewById<TextView>(Resource.Id.orderDetailsView_time_text);
            _linearLayoutTimeDaysValue = FindViewById<LinearLayout>(Resource.Id.linearLayout_timeDaysValue);
            _textViewTimeDays = FindViewById<TextView>(Resource.Id.time_days_text_view);
            _textViewTimeHours = FindViewById<TextView>(Resource.Id.time_hours_text_view);
            _textViewTimeMinutes = FindViewById<TextView>(Resource.Id.time_minutes_text_view);

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

            //askdjasdk
            bindingSet.Bind(_materialButtonTakeOrder)
                      .For(v => v.BindClick())
                      .To(vm => vm.TakeOrderCommand);
            bindingSet.Bind(_materialButtonTakeOrder)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsTakeOrderAvailable);
            bindingSet.Bind(_materialButtonSubribe)
                      .For(v => v.BindClick())
                      .To(vm => vm.SubscribeOrderCommand);
            bindingSet.Bind(_materialButtonSubribe)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsSubscribeAvailable);
            bindingSet.Bind(_materialButtonUnsubcribe)
                      .For(v => v.BindClick())
                      .To(vm => vm.UnsubscribeOrderCommand);
            bindingSet.Bind(_materialButtonUnsubcribe)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsUnsubscribeAvailable);
            bindingSet.Bind(_linearLayoutVideoLoad)
                      .For(v => v.BindVisible())
                      .To(vm => vm.VideoSectionViewModel.IsVideoLoadAvailable);
            bindingSet.Bind(_materialButtonLoadVideo)
                      .For(v => v.BindClick())
                      .To(vm => vm.VideoSectionViewModel.LoadVideoCommand);
            bindingSet.Bind(_materialButtonExecute)
                      .For(v => v.BindClick())
                      .To(vm => vm.ExecuteOrderCommand);
            bindingSet.Bind(_materialButtonExecute)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsExecuteOrderAvailable);
            bindingSet.Bind(_materialButtonCancel)
                      .For(v => v.BindClick())
                      .To(vm => vm.CancelOrderCommand);
            bindingSet.Bind(_materialButtonCancel)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsCancelOrderAvailable);
            bindingSet.Bind(_separator)
                      .For(v => v.BindVisible())
                      .To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_textViewTakeTheOrderText)
                      .For(v => v.BindVisible())
                      .To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_executorPhoto)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ExecutorSectionViewModel.ExecutorPhotoUrl);
            bindingSet.Bind(_executorPhoto)
                      .For(v => v.PlaceholderText)
                      .To(vm => vm.ExecutorSectionViewModel.ExecutorShortName);
            bindingSet.Bind(_executorPhoto)
                      .For(v => v.BindVisible())
                      .To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_executorPhoto)
                      .For(v => v.BindClick())
                      .To(vm => vm.ExecutorSectionViewModel.OpenExecutorProfileCommand);
            bindingSet.Bind(_textViewExecutorName)
                      .For(v => v.Text)
                      .To(vm => vm.ExecutorSectionViewModel.ExecutorName);
            bindingSet.Bind(_textViewExecutorName)
                      .For(v => v.BindVisible())
                      .To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_textViewTakeTheOrderDate)
                      .For(v => v.Text)
                      .To(vm => vm.StartOrderDate);
            bindingSet.Bind(_textViewTakeTheOrderDate)
                      .For(v => v.BindVisible())
                      .To(vm => vm.ExecutorSectionViewModel.IsExecutorAvailable);
            bindingSet.Bind(_cardViewFullVideo)
                      .For(v => v.BindClick())
                      .To(vm => vm.VideoSectionViewModel.ShowFullVideoCommand);
            bindingSet.Bind(_cardViewFullVideo)
                      .For(v => v.BindVisible())
                      .To(vm => vm.VideoSectionViewModel.IsVideoAvailable);
            bindingSet.Bind(_completedVideo)
                      .For(v => v.ImagePath)
                      .To(vm => vm.VideoSectionViewModel.VideoPlaceholderUrl);
            bindingSet.Bind(_processingView)
                      .For(v => v.BindVisible())
                      .To(vm => vm.VideoSectionViewModel.IsVideoProcessing);
            bindingSet.Bind(_percentRelativeLayoutVideo)
                      .For(v => v.BindVisible())
                      .To(vm => vm.VideoSectionViewModel.IsDecideVideoAvailable);
            bindingSet.Bind(_selectableButtonYes)
                      .For(v => v.BindClick())
                      .To(vm => vm.YesCommand);
            bindingSet.Bind(_selectableButtonYes)
                      .For(v => v.ArbitrationValue)
                      .To(vm => vm.SelectedArbitration);
            bindingSet.Bind(_selectableButtonYes)
                      .For(v => v.Text)
                      .To(vm => vm.YesText);
            bindingSet.Bind(_selectableButtonNo)
                      .For(v => v.BindClick())
                      .To(vm => vm.NoCommand);
            bindingSet.Bind(_selectableButtonNo)
                      .For(v => v.ArbitrationValue)
                      .To(vm => vm.SelectedArbitration);
            bindingSet.Bind(_selectableButtonNo)
                      .For(v => v.Text)
                      .To(vm => vm.NoText);
            bindingSet.Bind(_materialButtonAccept)
                      .For(v => v.BindClick())
                      .To(vm => vm.AcceptOrderCommand);
            bindingSet.Bind(_materialButtonAccept)
                      .For(v => v.BindVisible())
                      .To(vm => vm.VideoSectionViewModel.IsDecisionVideoAvailable);
            bindingSet.Bind(_materialButtonArqueOrder)
                      .For(v => v.BindClick())
                      .To(vm => vm.ArqueOrderCommand);
            bindingSet.Bind(_materialButtonArqueOrder)
                      .For(v => v.BindVisible())
                      .To(vm => vm.VideoSectionViewModel.IsDecisionVideoAvailable);
            bindingSet.Bind(_frameLayoutAnimation)
                      .For(v => v.BindVisible())
                      .To(vm => vm.VideoSectionViewModel.IsBusy);
            bindingSet.Bind(_mvxSwipeRefreshLayout)
                      .For(v => v.Refreshing)
                      .To(vm => vm.IsBusy).OneWay();
            bindingSet.Bind(_mvxSwipeRefreshLayout)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.LoadOrderDetailsCommand);
            bindingSet.Bind(_userPhoto)
                      .For(v => v.ImagePath)
                      .To(vm => vm.CustomerSectionViewModel.ProfilePhotoUrl).OneWay();
            bindingSet.Bind(_userPhoto)
                      .For(v => v.PlaceholderText)
                      .To(vm => vm.CustomerSectionViewModel.ProfileShortName).OneWay();
            bindingSet.Bind(_userPhoto)
                      .For(v => v.BindClick())
                      .To(vm => vm.CustomerSectionViewModel.OpenCustomerProfileCommand);
            bindingSet.Bind(_textViewProfileName)
                      .For(v => v.Text)
                      .To(vm => vm.CustomerSectionViewModel.ProfileName);
            bindingSet.Bind(_textViewOrderTitle)
                      .For(v => v.Text)
                      .To(vm => vm.OrderTitle);
            bindingSet.Bind(_viewOrderDescription)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsHiddenOrder);
            bindingSet.Bind(_textViewOrderDescription)
                      .For(v => v.Text)
                      .To(vm => vm.OrderDescription);
            bindingSet.Bind(_textViewPriceValue)
                      .For(v => v.Text)
                      .To(vm => vm.PriceValue);
            bindingSet.Bind(_textViewoOrderDetailsViewTime)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(_linearLayoutTimeDaysValue)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(_textViewTimeDays)
                      .For(v => v.Text)
                      .To(vm => vm.TimeDaysValue);
            bindingSet.Bind(_textViewTimeHours)
                      .For(v => v.Text)
                      .To(vm => vm.TimeHourValue);
            bindingSet.Bind(_textViewTimeMinutes)
                      .For(v => v.Text)
                      .To(vm => vm.TimeMinutesValue);


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
