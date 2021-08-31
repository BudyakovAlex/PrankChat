using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Core.ViewModels.Abstract
{
    public abstract class BasePageViewModel<TParameter> : BasePageViewModel, IMvxViewModel<TParameter>
    {
        public abstract void Prepare(TParameter parameter);
    }
}