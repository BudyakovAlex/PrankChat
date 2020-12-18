using System;
using AndroidX.RecyclerView.Widget;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    public class StateScrollListener : RecyclerView.OnScrollListener
    {
        public event EventHandler FinishScroll;

        public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
        {
            base.OnScrollStateChanged(recyclerView, newState);
            switch(newState)
            {
                case RecyclerView.ScrollStateIdle:
                    FinishScroll?.Invoke(this, null);
                    break;

                case RecyclerView.ScrollStateDragging:
                    break;

                case RecyclerView.ScrollStateSettling:
                    break;
            }
        }
    }
}
