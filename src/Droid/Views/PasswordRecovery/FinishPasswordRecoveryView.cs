using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.PasswordRecovery
{
    [MvxActivityPresentation]
    [Activity(
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/Theme.PrankChat.Base")]
    public class FinishPasswordRecoveryView : BaseView<FinishPasswordRecoveryViewModel>
    {
        private MaterialButton _finishPasswordRecoveryButton;
        private TextView _showPublicationTextView;

        protected override string TitleActionBar => Core.Localization.Resources.PasswordRecovery;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_finish_password_recovery);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_background);
        }

        protected override void Bind()
        {
            base.Bind();
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_finishPasswordRecoveryButton).For(v => v.BindClick()).To(vm => vm.ShowLoginCommand);
            bindingSet.Bind(_showPublicationTextView).For(v => v.BindClick()).To(vm => vm.ShowPublicationCommand);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _finishPasswordRecoveryButton = FindViewById<MaterialButton>(Resource.Id.finish_password_recovery_button);
            _showPublicationTextView = FindViewById<TextView>(Resource.Id.show_publication_text_view);
        }
    }
}
