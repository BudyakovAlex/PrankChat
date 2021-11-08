using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationParameters;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationResults;

namespace PrankChat.Mobile.Core.ViewModels.Abstract
{
    public abstract class BasePageViewModel<TParameter, TResult> : BasePageViewModelResult<TResult>, IMvxViewModel<GenericNavigationParams<TParameter>, GenericNavigationResult<TResult>>
    {
        public abstract void Prepare(TParameter parameter);

        void IMvxViewModel<GenericNavigationParams<TParameter>>.Prepare(GenericNavigationParams<TParameter> parameter) =>
            Prepare(parameter.Parameter);
    }
}