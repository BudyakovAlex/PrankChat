using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using PrankChat.Mobile.Droid.Utils.Helpers;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile
{
    [MvxActivityPresentation]
    [Activity]
    public class ProfileUpdateView : BaseView<ProfileUpdateViewModel>
    {
        private EditText _emailEditText;
        private ImageView _updateWarningImage;
        private TextView _resendConfirmationTextView;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.ProfileUpdateView_Title;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_profile_update);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_background);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _emailEditText = FindViewById<EditText>(Resource.Id.email_edit_text);
            _updateWarningImage = FindViewById<ImageView>(Resource.Id.update_warning_image);
            _resendConfirmationTextView = FindViewById<TextView>(Resource.Id.resend_confirmation_text_view);

            var textViewChangePassword = FindViewById<TextView>(Resource.Id.text_view_change_password);
            var textViewChangeProfilePhoto = FindViewById<TextView>(Resource.Id.text_view_change_photo);

            _resendConfirmationTextView.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
            textViewChangePassword.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
            textViewChangeProfilePhoto.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<ProfileUpdateView, ProfileUpdateViewModel>();

            bindingSet.Bind(_updateWarningImage)
                      .For(v => v.BindHidden())
                      .To(vm => vm.IsEmailVerified);

            bindingSet.Bind(_resendConfirmationTextView)
                      .For(v => v.BindHidden())
                      .To(vm => vm.IsEmailVerified)
                      .OneWay();

            bindingSet.Bind(_updateWarningImage)
                      .For(v => v.BindClick())
                      .To(vm => vm.ShowValidationWarningCommand);

            bindingSet.Bind(_resendConfirmationTextView)
                      .For(v => v.BindClick())
                      .To(vm => vm.ResendEmailValidationCommand);

            bindingSet.Bind(_emailEditText)
                      .For(PaddingTargetBinding.EndPadding)
                      .To(vm => vm.IsEmailVerified)
                      .WithConversion(BoolToIntConverter.Name, Tuple.Create(0, DisplayUtils.DpToPx(45)));

            bindingSet.Bind(_resendConfirmationTextView)
                      .For(v => v.Alpha)
                      .To(vm => vm.CanResendEmailValidation)
                      .WithConversion(BoolToFloatConverter.Name, Tuple.Create(1f, 0.5f));

            bindingSet.Apply();
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }
    }
}
