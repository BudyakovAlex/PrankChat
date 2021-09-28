using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using PrankChat.Mobile.Core.ViewModels.Dialogs;
using PrankChat.Mobile.Droid.Adapters;

namespace PrankChat.Mobile.Droid.Dialogs
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(ArrayDialog))]
    public class ArrayDialog : MvxDialogFragment<ArrayDialogViewModel>
    {
        private TextView _titleTextView;
        private MvxRecyclerView _titleRecyclerView;
        private TextView _closeTextView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNoFrame, Resource.Style.Theme_PrankChat_DialogStyle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.dialog_array, container);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.recycler_view);
            recyclerView.Adapter = new ArrayDialogRecyclerViewAdapter(BindingContext);

            _titleTextView = view.FindViewById<TextView>(Resource.Id.title_text_view);
            _titleRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.recycler_view);
            _closeTextView = view.FindViewById<TextView>(Resource.Id.close_text_view);
            Bind();
            return view;
        }

        protected void Bind()
        {
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_titleRecyclerView).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_titleRecyclerView).For(v => v.ItemClick).To(vm => vm.SelectItemCommand);
            bindingSet.Bind(_closeTextView).For(v => v.BindClick()).To(vm => vm.CloseCommand);
        }
    }
}
