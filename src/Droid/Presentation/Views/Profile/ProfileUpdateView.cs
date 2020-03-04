using Android.App;
using Android.OS;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile
{
    [MvxActivityPresentation]
    [Activity]
    public class ProfileUpdateView : BaseView<ProfileUpdateViewModel>
    {
        protected override string TitleActionBar => Core.Presentation.Localization.Resources.ProfileUpdateView_Title;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_profile_update);

            var textViewChangePassword = this.FindViewById<TextView>(Resource.Id.text_view_change_password);
            if (textViewChangePassword != null)
                textViewChangePassword.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;

            var textViewChangeProfilePhoto = this.FindViewById<TextView>(Resource.Id.text_view_change_photo);
            if (textViewChangeProfilePhoto != null)
                textViewChangeProfilePhoto.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }
    }
}
