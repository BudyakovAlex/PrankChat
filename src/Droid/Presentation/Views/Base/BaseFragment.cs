using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseFragment<TMvxViewModel> : MvxFragment<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        protected virtual bool HasBackButton => false;

        protected virtual string ActionTitle => string.Empty;

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
            title.Text = ActionTitle;

            var backButton = view.FindViewById<ImageButton>(Resource.Id.back_button);
            backButton.Visibility = HasBackButton ? ViewStates.Visible : ViewStates.Gone;

            return view;
        }

        protected abstract void Subscription();
		protected abstract void Unsubscription();

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
