using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationParameters;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationResults;

namespace PrankChat.Mobile.Core.ViewModels.Common
{
    public abstract class PaginationViewModel<TParameter, TResult, TItem> : PaginationViewModelResult<TResult, TItem>, IMvxViewModel<GenericNavigationParams<TParameter>, GenericNavigationResult<TResult>>
    {
        protected PaginationViewModel(int paginationSize)
            : base(paginationSize)
        {
        }

        public abstract void Prepare(TParameter parameter);

        void IMvxViewModel<GenericNavigationParams<TParameter>>.Prepare(GenericNavigationParams<TParameter> parameter) =>
           Prepare(parameter.Parameter);
    }
}
