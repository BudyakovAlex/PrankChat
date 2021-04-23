using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Search
{
    public class ProfileSearchItemViewHolder : CardViewHolder
    {
        private View _view;
        private CircleCachedImageView _profilePhotoImageView;
        private TextView _nameTextView;
        private TextView _descriptionTextView;

        public ProfileSearchItemViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _view = view;

            _profilePhotoImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.profile_photo);
            _nameTextView = view.FindViewById<TextView>(Resource.Id.name_text_view);
            _descriptionTextView = view.FindViewById<TextView>(Resource.Id.description_text_view);
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<ProfileSearchItemViewHolder, ProfileSearchItemViewModel>();

            bindingSet.Bind(_profilePhotoImageView).For(v => v.ImagePath).To(vm => vm.ImageUrl);
            bindingSet.Bind(_profilePhotoImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_view).For(v => v.BindClick()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(_nameTextView).For(v => v.Text).To(vm => vm.ProfileName);
            bindingSet.Bind(_descriptionTextView).For(v => v.Text).To(vm => vm.ProfileDescription);
        }
    }
}
