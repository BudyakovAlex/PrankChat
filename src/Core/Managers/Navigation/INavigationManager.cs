using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationParameters;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationResults;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Navigation
{
    public interface INavigationManager
    {
        event EventHandler<bool> IsNavigatingChanged;

        bool IsNavigating { get; }

        Task<bool> NavigateAsync<TViewModel>() where TViewModel : IMvxViewModel;

        Task<TResult> NavigateAsync<TViewModel, TResult>()
            where TViewModel : IMvxViewModelResult<GenericNavigationResult<TResult>>;

        Task<bool> NavigateAsync<TViewModel, TParameter>(TParameter parameter)
            where TViewModel : IMvxViewModel<GenericNavigationParams<TParameter>>;

        Task<TResult> NavigateAsync<TViewModel, TParameter, TResult>(TParameter parameter)
            where TViewModel : IMvxViewModel<GenericNavigationParams<TParameter>, GenericNavigationResult<TResult>>;

        Task<bool> CloseAsync(IMvxViewModel viewModel);

        Task<bool> CloseAsync<TResult>(IMvxViewModelResult<GenericNavigationResult<TResult>> viewModel, TResult result);
    }
}
