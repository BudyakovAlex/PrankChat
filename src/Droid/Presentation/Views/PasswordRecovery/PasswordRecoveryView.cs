using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.AppBar;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Plugin.Visibility;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.PasswordRecovery
{
    [MvxFragmentPresentation(
        Tag = nameof(PasswordRecoveryView),
        ActivityHostViewModelType = typeof(LoginViewModel),
        FragmentContentId = Resource.Id.container_layout,
        AddToBackStack = true)]
    [Register(nameof(PasswordRecoveryView))]
    public class PasswordRecoveryView : BaseFragment<PasswordRecoveryViewModel>
    {
        private ImageButton _imageButtonBack;
        private TextInputEditText _textInputEmailEditTextField;
        private MaterialButton _materialButtonRegistration;
        private ProgressBar _progressBar;

        public PasswordRecoveryView() : base(Resource.Layout.fragment_password_recovery)
        {
        }

        protected override bool HasBackButton => true;

        protected override void Bind()
        {
            base.Bind();
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_imageButtonBack).For(v => v.BindClick()).To(vm => vm.CloseCommand);
            bindingSet.Bind(_textInputEmailEditTextField).For(v => v.Text).To(vm => vm.Email);
            bindingSet.Bind(_materialButtonRegistration).For(v => v.BindClick()).To(vm => vm.RecoverPasswordCommand);
            bindingSet.Bind(_progressBar).For(v => v.BindVisible()).To(vm => vm.IsBusy);
            bindingSet.Bind(_progressBar).For(v => v.Visibility).To(vm => vm.IsBusy)
                .WithConversion<MvxVisibilityValueConverter>();
        }

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);
            _imageButtonBack = view.FindViewById<ImageButton>(Resource.Id.back_button);
            _textInputEmailEditTextField = view.FindViewById<TextInputEditText>(Resource.Id.email_edit_text_field);
            _materialButtonRegistration = view.FindViewById<MaterialButton>(Resource.Id.registration_button);
            _progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
        }
    }
}
