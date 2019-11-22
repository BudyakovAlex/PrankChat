using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Droid.PlatformServices;

namespace PrankChat.Mobile.Droid
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<IDialogService, DialogService>();
        }
    }
}
