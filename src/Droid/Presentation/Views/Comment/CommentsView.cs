using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Comment
{
    [MvxActivityPresentation]
    [Activity(WindowSoftInputMode = SoftInput.AdjustPan)]
    public class CommentsView : BaseView<CommentsViewModel>
    {
        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.CommentView_Title;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_comments_view);
        }
	}
}
