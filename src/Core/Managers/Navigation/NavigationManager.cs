using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Navigation
{
    public class NavigationManager : INavigationManager
    {
        private readonly IMvxNavigationService _navigationService;

        public NavigationManager(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            _navigationService.BeforeNavigate += OnBeforeNavigation;
            _navigationService.BeforeClose += OnBeforeNavigation;
            _navigationService.BeforeChangePresentation += OnBeforeNavigation;

            _navigationService.AfterNavigate += OnAfterNavigation;
            _navigationService.AfterClose += OnAfterNavigation;
            _navigationService.AfterChangePresentation += OnAfterNavigation;
        }

        public event EventHandler<bool> IsNavigatingChanged;

        public bool IsNavigating { get; private set; }

        public Task<bool> NavigateAsync<TViewModel>() where TViewModel : IMvxViewModel
        {
            return _navigationService.Navigate<TViewModel>();
        }

        public Task<TResult> NavigateAsync<TViewModel, TResult>() where TViewModel : IMvxViewModelResult<TResult>
        {
            return _navigationService.Navigate<TViewModel, TResult>();
        }

        public Task<bool> NavigateAsync<TViewModel, TParameter>(TParameter parameter) where TViewModel : IMvxViewModel<TParameter>
        {
            return _navigationService.Navigate<TViewModel, TParameter>(parameter);
        }

        public Task<TResult> NavigateAsync<TViewModel, TParameter, TResult>(TParameter parameter) where TViewModel : IMvxViewModel<TParameter, TResult>
        {
            return _navigationService.Navigate<TViewModel, TParameter, TResult>(parameter);
        }

        public Task<bool> CloseAsync(IMvxViewModel viewModel)
        {
            return _navigationService.Close(viewModel);
        }

        public Task<bool> CloseAsync<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result)
        {
            return _navigationService.Close(viewModel, result);
        }

        private void OnBeforeNavigation(object sender, object e)
        {
            IsNavigating = true;
            IsNavigatingChanged?.Invoke(this, IsNavigating);
        }

        private void OnAfterNavigation(object sender, object e)
        {
            IsNavigating = false;
            IsNavigatingChanged?.Invoke(this, IsNavigating);
        }
    }
}