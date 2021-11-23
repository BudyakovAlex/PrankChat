using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationParameters;

namespace PrankChat.Mobile.Core.ViewModels.Abstract
{
    public abstract class BasePageViewModel<TParameter> : BasePageViewModel, IMvxViewModel<GenericNavigationParams<TParameter>>
    {
        public abstract void Prepare(TParameter parameter);

        void IMvxViewModel<GenericNavigationParams<TParameter>>.Prepare(GenericNavigationParams<TParameter> parameter) =>
            Prepare(parameter.Parameter);
    }
}