using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class SearchView : BaseView<SearchViewModel>
    {
        private ClearEditText _searchTextView;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.search_layout);

            SetupControls();
            SetupBinding();
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }

        private void SetupControls()
        {
            _searchTextView = FindViewById<ClearEditText>(Resource.Id.search_text_view);

        }

        private void SetupBinding()
        {
            var set = this.CreateBindingSet<SearchView, SearchViewModel>();
            set.Bind(_searchTextView).To(vm => vm.SearchValue);
            set.Apply();
        }
    }
}
