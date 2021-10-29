using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Bindings;
using PrankChat.Mobile.Droid.Views.Base;
using PrankChat.Mobile.Droid.Utils.Helpers;

namespace PrankChat.Mobile.Droid.Views.Profile
{
    [MvxActivityPresentation]
    [Activity(
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/Theme.PrankChat.Base")]
    public class ProfileUpdateView : BaseView<ProfileUpdateViewModel>
    {
        private CircleCachedImageView _profileImageView;
        private EditText _emailEditText;
        private EditText _nameEditText;
        private EditText _loginEditText;
        private EditText _descriptionEditText;
        private ImageView _updateWarningImage;
        private TextView _resendConfirmationTextView;
        private TextView _limitTextView;
        private TextView _birthdayTextView;
        private View _birthdayContainerView;
        private RadioButton _maleRadioButton;
        private RadioButton _femaleRadioButton;
        private ProgressBar _progressBar;
        private Button _saveButton;
        private TextView _textViewChangeProfilePhoto;
        private TextView _textViewChangePassword;

        protected override string TitleActionBar => Core.Localization.Resources.ProfileEditing;

        protected override bool HasBackButton => true;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.profile_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.profile_menu_button:
                    ViewModel.ShowMenuCommand.Execute();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_profile_update);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_background);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _profileImageView = FindViewById<CircleCachedImageView>(Resource.Id.profile_image_view);
            _emailEditText = FindViewById<EditText>(Resource.Id.email_edit_text);
            _nameEditText = FindViewById<EditText>(Resource.Id.name_edit_text);
            _loginEditText = FindViewById<EditText>(Resource.Id.login_edit_text);
            _descriptionEditText = FindViewById<EditText>(Resource.Id.description_edit_text);
            _updateWarningImage = FindViewById<ImageView>(Resource.Id.update_warning_image);
            _resendConfirmationTextView = FindViewById<TextView>(Resource.Id.resend_confirmation_text_view);
            _limitTextView = FindViewById<TextView>(Resource.Id.limit_text_view);
            _birthdayTextView = FindViewById<TextView>(Resource.Id.birthday_text_view);
            _birthdayContainerView = FindViewById<View>(Resource.Id.birthday_container_view);
            _maleRadioButton = FindViewById<RadioButton>(Resource.Id.male_radio_button);
            _femaleRadioButton = FindViewById<RadioButton>(Resource.Id.female_radio_button);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            _saveButton = FindViewById<Button>(Resource.Id.save_button);

            _descriptionEditText.SetFilters(new[] { new InputFilterLengthFilter(Constants.Profile.DescriptionMaxLength) });

            _textViewChangeProfilePhoto = FindViewById<TextView>(Resource.Id.text_view_change_photo);
            _textViewChangePassword = FindViewById<TextView>(Resource.Id.text_view_change_password);

            _resendConfirmationTextView.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
            _textViewChangePassword.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
            _textViewChangeProfilePhoto.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<ProfileUpdateView, ProfileUpdateViewModel>();

            bindingSet.Bind(_updateWarningImage).For(v => v.BindHidden()).To(vm => vm.IsEmailVerified);
            bindingSet.Bind(_resendConfirmationTextView).For(v => v.BindHidden()).To(vm => vm.IsEmailVerified);
            bindingSet.Bind(_updateWarningImage).For(v => v.BindClick()).To(vm => vm.ShowValidationWarningCommand);
            bindingSet.Bind(_resendConfirmationTextView).For(v => v.BindClick()).To(vm => vm.ResendEmailValidationCommand);

            bindingSet.Bind(_birthdayTextView).For(v => v.Text).To(vm => vm.BirthdayText);
            bindingSet.Bind(_limitTextView).For(v => v.Text).To(vm => vm.LimitTextPresentation);
            bindingSet.Bind(_descriptionEditText).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_loginEditText).For(v => v.Text).To(vm => vm.Login);
            bindingSet.Bind(_nameEditText).For(v => v.Text).To(vm => vm.Name);
            bindingSet.Bind(_emailEditText).For(v => v.Text).To(vm => vm.Email);
            bindingSet.Bind(_emailEditText).For(v => v.BindPaddingEnd()).To(vm => vm.IsEmailVerified)
                      .WithConversion((bool value) => value ? 0 : DisplayUtils.DpToPx(45));
            bindingSet.Bind(_resendConfirmationTextView).For(v => v.Alpha).To(vm => vm.CanResendEmailValidation)
                      .WithConversion((bool value) => value ? 1 : 0.5f);

            bindingSet.Bind(_profileImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl);
            bindingSet.Bind(_profileImageView).For(v => v.BindClick()).To(vm => vm.ChangeProfilePhotoCommand);
            bindingSet.Bind(_profileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_textViewChangeProfilePhoto).For(v => v.BindClick()).To(vm => vm.ChangeProfilePhotoCommand);
            bindingSet.Bind(_birthdayContainerView).For(v => v.BindClick()).To(vm => vm.SelectBirthdayCommand);

            bindingSet.Bind(_maleRadioButton).For(v => v.Checked).To(vm => vm.IsGenderMale).OneWay();
            bindingSet.Bind(_maleRadioButton).For(v => v.BindClick()).To(vm => vm.SelectGenderCommand)
                      .CommandParameter(GenderType.Male);

            bindingSet.Bind(_femaleRadioButton).For(v => v.Checked).To(vm => vm.IsGenderFemale).OneWay();
            bindingSet.Bind(_femaleRadioButton).For(v => v.BindClick()).To(vm => vm.SelectGenderCommand)
                      .CommandParameter(GenderType.Female);

            bindingSet.Bind(_textViewChangePassword).For(v => v.BindClick()).To(vm => vm.ChangePasswordCommand);
            bindingSet.Bind(_saveButton).For(v => v.BindClick()).To(vm => vm.SaveProfileCommand);
            bindingSet.Bind(_progressBar).For(v => v.BindVisible()).To(vm => vm.IsBusy);
        }
    }
}