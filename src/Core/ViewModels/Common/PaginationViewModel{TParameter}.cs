using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Core.ViewModels.Common
{
    public abstract class PaginationViewModel<TParameter> : PaginationViewModel, IMvxViewModel<TParameter>
    {
        protected PaginationViewModel(int paginationSize)
            : base(paginationSize)
        {

        }

        public abstract void Prepare(TParameter parameter);
    }
}
