using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationResults;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ViewModels.Common
{
    public abstract class PaginationViewModelResult<TResult, TItem> : PaginationViewModel<TItem>, IMvxViewModelResult<GenericNavigationResult<TResult>>
    {
        protected PaginationViewModelResult(int paginationSize)
            : base(paginationSize)
        {

        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        protected virtual TResult DefaultResult { get; }

        protected override async Task CloseAsync(bool? isPlatform)
        {
            if (isPlatform == true)
            {
                return;
            }

            var isCloseConfirmed = await ConfirmPlatformCloseAsync().ConfigureAwait(false);
            if (!isCloseConfirmed)
            {
                return;
            }

            await NavigationManager.CloseAsync(this, DefaultResult).ConfigureAwait(false);
        }
    }
}
