using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Arbitration
{
    public class ArbitrationItemViewHolder : CardViewHolder<ArbitrationItemViewModel>
    {
        public ArbitrationItemViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }
    }
}
