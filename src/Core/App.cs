using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<MainViewModel>();
        }
    }
}
