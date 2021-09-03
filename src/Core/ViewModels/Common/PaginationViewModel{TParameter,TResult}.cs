using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Core.ViewModels.Common
{
    public abstract class PaginationViewModel<TParameter, TResult> : PaginationViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    {
        public PaginationViewModel(int paginationSize) : base(paginationSize)
        {
            
        }

        public abstract void Prepare(TParameter parameter);
    }
}
