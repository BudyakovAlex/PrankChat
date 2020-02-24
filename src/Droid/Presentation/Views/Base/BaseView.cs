﻿using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseView<TMvxViewModel> : MvxAppCompatActivity<TMvxViewModel>, IToolbarOwner
        where TMvxViewModel : BaseViewModel
    {
        public Toolbar Toolbar { get; private set; }

        protected virtual bool HasBackButton => false;

        protected virtual bool HasActionBarVisible => true;

        protected virtual string TitleActionBar => string.Empty;

		protected virtual void OnCreate(Bundle savedInstanceState, int layoutId)
        {
            base.OnCreate(savedInstanceState);

            RequestedOrientation = ScreenOrientation.Portrait;

            SetContentView(layoutId);
            SetViewProperties();

            Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar == null)
            {
                DoBind();
                return;
            }

            SetSupportActionBar(Toolbar);
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

            var title = FindViewById<TextView>(Resource.Id.toolbar_title);
            if (title != null)
                title.Text = TitleActionBar;
        }

        protected virtual void Subscription()
        {
        }

        protected virtual void Unsubscription()
        {
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

        protected virtual void DoBind()
        {
        }

        protected virtual void SetViewProperties()
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
            ViewModel.GoBackCommand.Execute();
        }
    }
}
