using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Registration
{
    [MvxActivityPresentation]
    [Activity(
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/Theme.PrankChat.Base")]
    public class RegistrationView : BaseView<RegistrationViewModel>
    {
        protected override bool HasBackButton => true;

        private TextInputEditText _emailInputEditText;
        private ImageButton _vkImageButton;
        private ImageButton _okImageButton;
        private ImageButton _facebookImageButton;
        private ImageButton _gmailImageButton;
        private TextView _goToLoginTextView;
        private MaterialButton _registrationButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_registration);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_background);

            var textViewLogin = FindViewById<TextView>(Resource.Id.go_to_login_text_view);
            textViewLogin.PaintFlags |= Android.Graphics.PaintFlags.UnderlineText;
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _emailInputEditText = FindViewById<TextInputEditText>(Resource.Id.email_input_edit_text);
            _vkImageButton = FindViewById<ImageButton>(Resource.Id.vk_image_button);
            _okImageButton = FindViewById<ImageButton>(Resource.Id.ok_image_button);
            _facebookImageButton = FindViewById<ImageButton>(Resource.Id.facebook_login_image_button);
            _gmailImageButton = FindViewById<ImageButton>(Resource.Id.gmail_login_image_button);
            _goToLoginTextView = FindViewById<TextView>(Resource.Id.go_to_login_text_view);
            _registrationButton = FindViewById<MaterialButton>(Resource.Id.registration_button);
        }

        protected override void Bind()
        {
            base.Bind();
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_emailInputEditText).For(v => v.Text).To(vm => vm.Email);
            bindingSet.Bind(_vkImageButton).For(v => v.BindClick()).To(vm => vm.LoginCommand).CommandParameter(LoginType.Vk);
            bindingSet.Bind(_okImageButton).For(v => v.BindClick()).To(vm => vm.LoginCommand).CommandParameter(LoginType.Ok);
            bindingSet.Bind(_facebookImageButton).For(v => v.BindClick()).To(vm => vm.LoginCommand).CommandParameter(LoginType.Facebook);
            bindingSet.Bind(_gmailImageButton).For(v => v.BindClick()).To(vm => vm.LoginCommand).CommandParameter(LoginType.Gmail);
            bindingSet.Bind(_goToLoginTextView).For(v => v.BindClick()).To(vm => vm.CloseCommand);
            bindingSet.Bind(_registrationButton).For(v => v.BindClick()).To(vm => vm.ShowSecondStepCommand);
        }
    }
}