using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Navigation
{
    public interface INavigationManager
    {
        event EventHandler<bool> IsNavigatingChanged;

        bool IsNavigating { get; }

        Task<bool> NavigateAsync<TViewModel>() where TViewModel : IMvxViewModel;

        Task<TResult> NavigateAsync<TViewModel, TResult>() where TViewModel : IMvxViewModelResult<TResult>;

        Task<bool> NavigateAsync<TViewModel, TParameter>(TParameter parameter) where TViewModel : IMvxViewModel<TParameter>;

        Task<TResult> NavigateAsync<TViewModel, TParameter, TResult>(TParameter parameter) where TViewModel : IMvxViewModel<TParameter, TResult>;

        Task<bool> CloseAsync(IMvxViewModel viewModel);

        Task<bool> CloseAsync<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result);
    }
}
