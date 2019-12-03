using Acr.UserDialogs;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseView<TMvxViewModel> : MvxAppCompatActivity<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        protected virtual bool HasBackButton => false;

        protected virtual bool HasActionBarVisible => true;

		protected abstract void Subscription();
		protected abstract void Unsubscription();

		protected virtual void OnCreate(Bundle savedInstanceState, int layoutId)
        {
            base.OnCreate(savedInstanceState);

            RequestedOrientation = ScreenOrientation.Portrait;

            SetContentView(layoutId);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (toolbar == null)
            {
                return;
            }

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowCustomEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(HasBackButton);
            SupportActionBar.SetDisplayShowHomeEnabled(!HasBackButton);
            SupportActionBar.SetDisplayUseLogoEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            if (HasActionBarVisible)
            {
                SupportActionBar.Show();
            }
            else
            {
                SupportActionBar.Hide();
            }

            InitServices();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    ViewModel.GoBackCommand.Execute();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

		protected override void OnStart()
		{
			base.OnStart();
			Subscription();
		}

		protected override void OnStop()
		{
			base.OnStop();
			Unsubscription();
		}

		public override void OnBackPressed()
        {
            ViewModel.GoBackCommand.Execute();
        }

        private void InitServices()
        {
            UserDialogs.Init(this);
        }
    }
}
