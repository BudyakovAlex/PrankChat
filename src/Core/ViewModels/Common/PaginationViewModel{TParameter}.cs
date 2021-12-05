using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationParameters;

namespace PrankChat.Mobile.Core.ViewModels.Common
{
    public abstract class PaginationViewModel<TParameter, TItem> : PaginationViewModel<TItem>, IMvxViewModel<GenericNavigationParams<TParameter>>
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
