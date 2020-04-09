using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Rating
{
    public class RatingItemViewHolder : CardViewHolder<RatingItemViewModel>
    {
        public RatingItemViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }
    }
}
