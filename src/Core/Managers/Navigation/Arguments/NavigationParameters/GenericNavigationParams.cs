namespace PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationParameters
{
    public class GenericNavigationParams<TParameter>
    {
        public GenericNavigationParams(TParameter parameter)
        {
            Parameter = parameter;
        }

        public TParameter Parameter { get; }
    }
}
