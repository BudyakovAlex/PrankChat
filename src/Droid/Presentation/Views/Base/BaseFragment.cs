using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseFragment<TMvxViewModel> : MvxFragment<TMvxViewModel>, IToolbarOwner
        where TMvxViewModel : BasePageViewModel
    {
        private readonly int _layoutId;

        public BaseFragment(int layoutId)
        {
            _layoutId = layoutId;
        }

        public Toolbar Toolbar { get; private set; }

        protected virtual bool HasBackButton => false;

        protected virtual string TitleActionBar => string.Empty;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(_layoutId, null);

            SetViewProperties(view);

            Toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar == null)
            {
                Bind();
                return view;
            }

            var title = view.FindViewById<TextView>(Resource.Id.toolbar_title);
            if (title != null)
            {
                title.Text = TitleActionBar;
            }

            var backButton = view.FindViewById<ImageButton>(Resource.Id.back_button);
            if (backButton != null)
            {
                backButton.Visibility = HasBackButton ? ViewStates.Visible : ViewStates.Gone;
            }

            Bind();
            return view;
        }

        protected virtual void SetViewProperties(View view)
        {
        }

        protected virtual void Bind()
        {
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
