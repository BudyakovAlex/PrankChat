using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;

namespace PrankChat.Mobile.Droid.Presentation.Dialogs
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(ShareDialog))]
    public class ShareDialog : MvxDialogFragment<ShareDialogViewModel>
    {
        private ImageButton _shareButton;
        private ImageButton _copyLinkButton;
        private ImageButton _shareInstButton;
        private MaterialButton _closeButton;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNoFrame, Resource.Style.Theme_PrankChat_ShareDialogStyle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            Dialog.Window.SetGravity(GravityFlags.Bottom);

            var view = this.BindingInflate(Resource.Layout.dialog_share, null);
            Bind();
            return view;
        }

        private void Bind()
        {
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_shareButton).For(v => v.BindClick()).To(vm => vm.ShareCommand);
            bindingSet.Bind(_copyLinkButton).For(v => v.BindClick()).To(vm => vm.CopyLinkCommand);
            bindingSet.Bind(_shareInstButton).For(v => v.BindClick()).To(vm => vm.ShareToInstagramCommand);
            bindingSet.Bind(_closeButton).For(v => v.BindClick()).To(vm => vm.CloseCommand);
        }
    }
}