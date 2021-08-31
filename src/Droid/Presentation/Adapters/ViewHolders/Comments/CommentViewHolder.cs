using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.ViewModels.Comment.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Comments
{
    public class CommentViewHolder : CardViewHolder
    {
        private CircleCachedImageView _profileImageView;
        private TextView _userNameTextView;
        private TextView _commentTextView;
        private TextView _dateTextView;

        public CommentViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _profileImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.profile_image_view);
            _userNameTextView = view.FindViewById<TextView>(Resource.Id.user_name_text_view);
            _commentTextView = view.FindViewById<TextView>(Resource.Id.comment_text_view);
            _dateTextView = view.FindViewById<TextView>(Resource.Id.post_date_text_view);
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<CommentViewHolder, CommentItemViewModel>();

            bindingSet.Bind(_profileImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl);
            bindingSet.Bind(_profileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_profileImageView).For(v => v.BindClick()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(_userNameTextView).For(v => v.Text).To(vm => vm.ProfileName);
            bindingSet.Bind(_commentTextView).For(v => v.Text).To(vm => vm.Comment);
            bindingSet.Bind(_dateTextView).For(v => v.Text).To(vm => vm.DateText);
        }

    }
}
