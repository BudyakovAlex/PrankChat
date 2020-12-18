using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;
using PrankChat.Mobile.Droid.Presentation.Adapters;

namespace PrankChat.Mobile.Droid.Presentation.Dialogs
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(ArrayDialog))]
    public class ArrayDialog : MvxDialogFragment<ArrayDialogViewModel>
    {
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

            return view;
        }
    }
}
