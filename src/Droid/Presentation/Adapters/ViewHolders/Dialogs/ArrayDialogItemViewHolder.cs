using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Dialogs
{
    public class ArrayDialogItemViewHolder : MvxRecyclerViewHolder
    {
        private TextView _textView;

        public ArrayDialogItemViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            Initialize(itemView);
            this.DelayBind(Bind);
        }

        private void Initialize(View view)
        {
            _textView = view.FindViewById<TextView>(Resource.Id.text_view);
        }

        private void Bind()
        {
            using var bindingSet = this.CreateBindingSet<ArrayDialogItemViewHolder, string>();
            bindingSet.Bind(_textView).For(v => v.Text).To(vm => vm);
        }
    }
}
