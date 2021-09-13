using System;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;

namespace PrankChat.Mobile.Droid.Adapters.TemplateSelectors
{
    public interface ITemplateSelector : IMvxTemplateSelector
    {
        Type GetItemViewHolderType(int templateId);
    }
}
