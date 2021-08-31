using Android.App;
using Android.OS;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Walthroughs;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Walthroughs
{
    [Activity(Theme = "@style/Theme.PrankChat.Translucent")]
    [MvxActivityPresentation]
    public class WalthroughView : BaseView<WalthroughViewModel>
    {
        private TextView _titleTextView;
        private TextView _descriptionTextView;
        private ImageView _closeImageView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_walkthrouth);
        }

        protected override void SetViewProperties()
        {
            _closeImageView = FindViewById<ImageView>(Resource.Id.close_image);
            _titleTextView = FindViewById<TextView>(Resource.Id.title_text_view);
            _descriptionTextView = FindViewById<TextView>(Resource.Id.description_text_view);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<WalthroughView, WalthroughViewModel>();

            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_descriptionTextView).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_closeImageView).For(v => v.BindClick()).To(vm => vm.CloseCommand);
        }
    }
}