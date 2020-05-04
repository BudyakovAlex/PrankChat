using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Dialogs;

namespace PrankChat.Mobile.Droid.Presentation.Adapters
{
    public class ArrayDialogRecyclerViewAdapter : MvxRecyclerAdapter
    {
        public ArrayDialogRecyclerViewAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        {
        }

        public ArrayDialogRecyclerViewAdapter(IMvxBindingContext bindingContext)
            : this(bindingContext as IMvxAndroidBindingContext)
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var context = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var view = context.BindingInflate(Resource.Layout.cell_array_dialog, parent, false);
            var viewHolder = new ArrayDialogItemViewHolder(view, context);

            return viewHolder;
        }
    }
}
