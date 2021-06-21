using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Ioc;

namespace PrankChat.Mobile.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            var compositionRoot = new CompositionRoot(false);
            compositionRoot.Initialize();

            RegisterCustomAppStart<CustomAppStart>();
        }
    }
}