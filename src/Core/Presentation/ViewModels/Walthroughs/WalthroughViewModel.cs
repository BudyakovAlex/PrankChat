using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Walthroughs
{
    public class WalthroughViewModel : BasePageViewModel, IMvxViewModel<WalthroughNavigationParameter>
    {
        public WalthroughViewModel()
        {
            CloseCommand = new MvxCommand(() => NavigationService.CloseView(this));
        }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public ICommand CloseCommand { get; }

        public void Prepare(WalthroughNavigationParameter parameter)
        {
            Title = parameter.Title;
            Description = parameter.Description;
        }
    }
}