using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Core.ViewModels.Abstract
{
    public abstract class BasePageViewModel<TParameter, TResult> : BasePageViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    {
        public abstract void Prepare(TParameter parameter);
    }
}