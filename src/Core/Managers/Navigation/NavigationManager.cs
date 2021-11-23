using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationParameters;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationResults;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Navigation
{
    public class NavigationManager : INavigationManager
    {
        private readonly IMvxNavigationService navigationService;

        public NavigationManager(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;

            this.navigationService.WillNavigate += OnWillNavigate;
            this.navigationService.WillClose += OnWillNavigate;
            this.navigationService.WillChangePresentation += OnWillNavigate;

            this.navigationService.DidNavigate += OnDidNavigate;
            this.navigationService.DidClose += OnDidNavigate;
            this.navigationService.DidChangePresentation += OnDidNavigate;
        }

        public event EventHandler<bool> IsNavigatingChanged;

        public bool IsNavigating { get; private set; }

        public Task<bool> NavigateAsync<TViewModel>() where TViewModel : IMvxViewModel
        {
            return navigationService.Navigate<TViewModel>();
        }

        public async Task<TResult> NavigateAsync<TViewModel, TResult>() where TViewModel : IMvxViewModelResult<GenericNavigationResult<TResult>>
        {
            var result = await navigationService.Navigate<TViewModel, GenericNavigationResult<TResult>>();
            return result.Result;
        }

        public Task<bool> NavigateAsync<TViewModel, TParameter>(TParameter parameter) where TViewModel : IMvxViewModel<GenericNavigationParams<TParameter>>
        {
            return navigationService.Navigate<TViewModel, GenericNavigationParams<TParameter>>(new GenericNavigationParams<TParameter>(parameter));
        }

        public async Task<TResult> NavigateAsync<TViewModel, TParameter, TResult>(TParameter parameter) where TViewModel : IMvxViewModel<GenericNavigationParams<TParameter>, GenericNavigationResult<TResult>>
        {
            var result = await navigationService.Navigate<TViewModel, GenericNavigationParams<TParameter>, GenericNavigationResult<TResult>>(new GenericNavigationParams<TParameter>(parameter));
            return result.Result;
        }

        public Task<bool> CloseAsync(IMvxViewModel viewModel)
        {
            return navigationService.Close(viewModel);
        }

        public Task<bool> CloseAsync<TResult>(IMvxViewModelResult<GenericNavigationResult<TResult>> viewModel, TResult result)
        {
            return navigationService.Close(viewModel, new GenericNavigationResult<TResult>(result));
        }

        private void OnWillNavigate(object sender, object e)
        {
            IsNavigating = true;
            IsNavigatingChanged?.Invoke(this, IsNavigating);
        }

        private void OnDidNavigate(object sender, object e)
        {
            IsNavigating = false;
            IsNavigatingChanged?.Invoke(this, IsNavigating);
        }
    }
}