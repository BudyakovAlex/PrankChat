using System;
using System.Diagnostics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Publications
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Publications", ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(PublicationsView))]
    public class PublicationsView : BaseTabFragment<PublicationsViewModel>
    {
		private TabLayout _publicationTypeTabLayout;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.publications_layout, null);
			InitializeControls(view);
			return view;
        }

		public override void OnStart()
		{
			base.OnStart();
			Subscription();
		}

		public override void OnStop()
		{
			base.OnStop();
			Unsubscription();
		}

		private void InitializeControls(View view)
		{
			_publicationTypeTabLayout = view.FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
		}

		private void Subscription()
		{
			_publicationTypeTabLayout.TabSelected += PublicationTypeTabLayoutTabSelected;
		}

		private void Unsubscription()
		{
			_publicationTypeTabLayout.TabSelected -= PublicationTypeTabLayoutTabSelected;
		}

		private void PublicationTypeTabLayoutTabSelected(object sender, TabLayout.TabSelectedEventArgs e)
		{
			var publicationType = (PublicationType) e.Tab.Position;
			ViewModel.SelectedPublicationType = publicationType; 
		}
	}
}
