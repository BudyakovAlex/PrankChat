using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;

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
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.dialog_array, container);
            return view;
        }
    }
}
