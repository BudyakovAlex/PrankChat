using AndroidX.RecyclerView.Widget;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract
{
    public interface INestedCardViewHolder
    {
        int RecycledViewsVisibleCount { get; }

        RecyclerView NestedRecyclerView { get; }
    }
}
