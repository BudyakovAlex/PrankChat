using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseFragment<TMvxViewModel> : MvxFragment<TMvxViewModel> where TMvxViewModel : class, IMvxViewModel
    {
        protected virtual bool HasBackButton => false;

        protected virtual string TitleActionBar => string.Empty;

        public virtual View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState, int resourceId)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(resourceId, null);

            var toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar == null)
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
