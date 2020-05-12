﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Publications
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class PublicationDetailsView : BaseView<PublicationDetailsViewModel>
    {
        protected override bool HasBackButton => true;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_publication_details);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
        }
    }
}
