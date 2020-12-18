using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseFragment<TMvxViewModel> : MvxFragment<TMvxViewModel>, IToolbarOwner
        where TMvxViewModel : BasePageViewModel
    {
        public Toolbar Toolbar { get; private set; }

        protected virtual bool HasBackButton => false;

        protected virtual string TitleActionBar => string.Empty;

        public virtual View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState, int resourceId)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(resourceId, null);

            Toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar == null)
            {
                return view;
            }

            var title = view.FindViewById<TextView>(Resource.Id.toolbar_title);
            if (title != null)
                title.Text = TitleActionBar;

            var backButton = view.FindViewById<ImageButton>(Resource.Id.back_button);
            if (backButton != null)
                backButton.Visibility = HasBackButton ? ViewStates.Visible : ViewStates.Gone;

            return view;
        }

        protected virtual void Subscription()
        {
        }

		protected virtual void Unsubscription()
        {
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
	}
}
