using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Plugin.Visibility;
using PrankChat.Mobile.Core.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.PasswordRecovery
{
    [MvxActivityPresentation]
    [Activity(
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/Theme.PrankChat.Base")]
    public class PasswordRecoveryView : BaseView<PasswordRecoveryViewModel>
    {
        private ImageButton _backImageButton;
        private TextInputEditText _textInputEmailEditText;
        private MaterialButton _registrationButton;
        private ProgressBar _progressBar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_password_recovery);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_background);
        }

        protected override void Bind()
        {
            base.Bind();
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_backImageButton).For(v => v.BindClick()).To(vm => vm.CloseCommand);
            bindingSet.Bind(_textInputEmailEditText).For(v => v.Text).To(vm => vm.Email);
            bindingSet.Bind(_registrationButton).For(v => v.BindClick()).To(vm => vm.RecoverPasswordCommand);
            bindingSet.Bind(_progressBar).For(v => v.BindVisible()).To(vm => vm.IsBusy);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _backImageButton = FindViewById<ImageButton>(Resource.Id.back_button);
            _textInputEmailEditText = FindViewById<TextInputEditText>(Resource.Id.email_edit_text);
            _registrationButton = FindViewById<MaterialButton>(Resource.Id.registration_button);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
        }
    }
}
