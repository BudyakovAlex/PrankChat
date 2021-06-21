using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Notifications
{
    public class NotificationItemViewHolder : CardViewHolder
    {
        private View _view;
        private CircleCachedImageView _profilePhotoImageView;
        private View _badgeView;
        private NotificationTextView _titleTextView;
        private TextView _descriptionTextView;
        private TextView _dateTextView;

        public NotificationItemViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _view = view;

            _profilePhotoImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.profile_photo);
            _badgeView = view.FindViewById<View>(Resource.Id.badge_view);
            _titleTextView = view.FindViewById<NotificationTextView>(Resource.Id.title_text_view);
            _descriptionTextView = view.FindViewById<TextView>(Resource.Id.description_text_view);
            _dateTextView = view.FindViewById<TextView>(Resource.Id.date_text_view);
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<NotificationItemViewHolder, NotificationItemViewModel>();

            bindingSet.Bind(_profilePhotoImageView).For(v => v.ImagePath).To(vm => vm.ImageUrl);
            bindingSet.Bind(_profilePhotoImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_badgeView).For(v => v.BindHidden()).To(vm => vm.IsDelivered);

            bindingSet.Bind(_titleTextView).For(v => v.Title).To(vm => vm.Title);
            bindingSet.Bind(_titleTextView).For(v => v.ProfileName).To(vm => vm.ProfileName);

            bindingSet.Bind(_descriptionTextView).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_descriptionTextView).For(v => v.BindVisible()).To(vm => vm.HasDescription);
            bindingSet.Bind(_dateTextView).For(v => v.Text).To(vm => vm.DateText);
            bindingSet.Bind(_view).For(v => v.BindClick()).To(vm => vm.OpenUserProfileCommand);
        }
    }
}
