using Android.Content.PM;
using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Views;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace PrankChat.Mobile.Droid.Views.Base
{
    public abstract class BaseView<TMvxViewModel> : MvxActivity<TMvxViewModel>, IToolbarOwner
        where TMvxViewModel : BasePageViewModel
    {
        public Toolbar Toolbar { get; private set; }

        protected virtual bool HasBackButton => false;

        protected virtual bool HasActionBarVisible => true;

        protected virtual string TitleActionBar => string.Empty;

		protected virtual void OnCreate(Bundle savedInstanceState, int layoutId)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                SetContentView(layoutId);
                SetViewProperties();

                Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (Toolbar == null)
                {
                    return;
                }

                SetSupportActionBar(Toolbar);
                Toolbar.Title = TitleActionBar;
                SupportActionBar.SetDisplayShowCustomEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.SetDisplayHomeAsUpEnabled(HasBackButton);
                SupportActionBar.SetDisplayShowHomeEnabled(!HasBackButton);
                SupportActionBar.SetDisplayUseLogoEnabled(true);

                if (HasActionBarVisible)
                {
                    SupportActionBar.Show();
                }
                else
                {
                    SupportActionBar.Hide();
                }
            }
            finally
            {
                Bind();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    ViewModel.CloseCommand.Execute(null);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected virtual void Bind()
        {
        }

        protected virtual void SetViewProperties()
        {
        }

        protected virtual void Subscription()
        {
        }

        protected virtual void Unsubscription()
        {
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
            ViewModel.CloseCommand.Execute(null);
        }
    }
}
