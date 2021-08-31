using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Droid.Adapters.ViewHolders.Abstract
{
    public abstract class CardViewHolder<TViewModel> : CardViewHolder
        where TViewModel : MvxNotifyPropertyChanged
    {
        public CardViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        public TViewModel ViewModel => BindingContext.DataContext as TViewModel;
    }
}
