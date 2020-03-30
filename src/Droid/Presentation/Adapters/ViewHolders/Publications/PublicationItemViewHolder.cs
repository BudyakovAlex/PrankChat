using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract.Video;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Publications
{
    public class PublicationItemViewHolder : VideoCardViewHolder<PublicationItemViewModel>
    {
        public PublicationItemViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }
    }
}