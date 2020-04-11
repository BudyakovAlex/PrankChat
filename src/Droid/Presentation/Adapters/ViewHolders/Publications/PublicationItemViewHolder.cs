using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract.Video;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Publications
{
    public class PublicationItemViewHolder : VideoCardViewHolder<PublicationItemViewModel>
    {
        public PublicationItemViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<PublicationItemViewHolder, PublicationItemViewModel>();

            bindingSet.Bind(ProceccingView)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsVideoProcessing);

            bindingSet.Bind(VideoView)
                      .For(v => v.BindHidden())
                      .To(vm => vm.IsVideoProcessing);

            bindingSet.Bind(this)
                      .For(v => v.CanShowStub)
                      .To(vm => vm.IsVideoProcessing)
                      .WithConversion<MvxInvertedBooleanConverter>();

            bindingSet.Apply();
        }
    }
}