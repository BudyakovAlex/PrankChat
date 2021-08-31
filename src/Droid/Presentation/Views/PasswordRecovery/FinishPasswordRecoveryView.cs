using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.PasswordRecovery
{
    [MvxFragmentPresentation(
        Tag = nameof(FinishPasswordRecoveryView),
        ActivityHostViewModelType = typeof(LoginViewModel),
        FragmentContentId = Resource.Id.container_layout,
        AddToBackStack = true)]
    [Register(nameof(FinishPasswordRecoveryView))]
    public class FinishPasswordRecoveryView : BaseFragment<FinishPasswordRecoveryViewModel>
    {
        private MaterialButton _finishPasswordRecoveryButton;
        private TextView _showPublicationTextView;

        public FinishPasswordRecoveryView() : base(Resource.Layout.fragment_finish_password_recovery)
        {
        }

        protected override bool HasBackButton => false;

        protected override string TitleActionBar => Core.Localization.Resources.Password_Recovery_View_Title;

        protected override void Bind()
        {
            base.Bind();
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_finishPasswordRecoveryButton).For(v => v.BindClick()).To(vm => vm.ShowLoginCommand);
            bindingSet.Bind(_showPublicationTextView).For(v => v.BindClick()).To(vm => vm.ShowPublicationCommand);
        }

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _finishPasswordRecoveryButton = view.FindViewById<MaterialButton>(Resource.Id.finish_password_recovery_button);
            _showPublicationTextView = view.FindViewById<TextView>(Resource.Id.show_publication_text_view);
        }
    }
}
