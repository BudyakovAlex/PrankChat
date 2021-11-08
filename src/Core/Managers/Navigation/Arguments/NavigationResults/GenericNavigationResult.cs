namespace PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationResults
{
    public class GenericNavigationResult<TResult>
    {
        public GenericNavigationResult(TResult result)
        {
            Result = result;
        }

        public TResult Result { get; }
    }
}
