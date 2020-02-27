using System;
using System.Linq;
using Android.Runtime;
using Android.Support.V7.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using Object = Java.Lang.Object;

namespace PrankChat.Mobile.Droid.Presentation.Views.Publications
{
    public class PublicationsRecyclerAdapter : MvxRecyclerAdapter
    {
        private const int InactivatedPosition = -1;

        public PublicationsRecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public PublicationsRecyclerAdapter() : base(MvxAndroidBindingContextHelpers.Current())
        {
        }

        public PublicationsRecyclerAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {
        }

        public override void OnViewRecycled(Object holder)
        {
            if (BindingContext.DataContext is PublicationsViewModel viewModel
                && holder is RecyclerView.ViewHolder viewHolder
                && viewHolder.LayoutPosition != InactivatedPosition)
            {
                var itemViewModel = viewModel.Items.ElementAtOrDefault(viewHolder.LayoutPosition);
                StopVideo(itemViewModel);
            }

            base.OnViewRecycled(holder);
        }

        private void StopVideo(PublicationItemViewModel itemViewModel)
        {
            var videoService = itemViewModel?.VideoPlayerService;
            videoService?.Stop();
        }
    }
}
