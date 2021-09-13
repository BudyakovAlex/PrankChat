using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Publication;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Views.Base;

namespace PrankChat.Mobile.iOS.Views.Publication
{
    [MvxModalPresentation(WrapInNavigationController = true)]
	public partial class PublicationDetailsView : BaseGradientBarView<PublicationDetailsViewModel>
	{
		protected override void Bind()
		{
			using var bindingSet = this.CreateBindingSet<PublicationDetailsView, PublicationDetailsViewModel>();

			bindingSet.Bind(commentatorNameLabel).To(vm => vm.CommentatorName);
			bindingSet.Bind(commentatorPhotoImageView).For(v => v.ImagePath).To(vm => vm.CommentatorPhotoUrl)
				.Mode(MvxBindingMode.OneTime);
			//TODO: uncomment when vm will be ready
			//set.Bind(commentatorPhotoImageView)
			//   .For(v => v.BindTap())
			//   .To(vm => vm.OpenUserProfileCommand);
			bindingSet.Bind(commentLabel).To(vm => vm.NumberOfCommentText)
				.Mode(MvxBindingMode.OneTime);
			bindingSet.Bind(commentView.Tap()).For(v => v.Command).To(vm => vm.OpenCommentsCommand)
				.Mode(MvxBindingMode.OneTime);
			bindingSet.Bind(commentDateLabel).To(vm => vm.CommentDateText)
				.Mode(MvxBindingMode.OneTime);
			//TODO: uncomment when vm will be ready
			//set.Bind(likeLabel)
			//	.To(vm => vm.NumberOfLikesText)
			//	.Mode(MvxBindingMode.OneTime);
			//set.Bind(profileNameLabel)
			//	.To(vm => vm.ProfileName)
			//	.Mode(MvxBindingMode.OneTime);
			//set.Bind(profilePhotoImageView)
			//	.For(v => v.ImagePath)
			//	.To(vm => vm.ProfilePhotoUrl)
			//	.Mode(MvxBindingMode.OneTime);
			//set.Bind(profilePhotoImageView)
			//   .For(v => v.PlaceholderText)
			//   .To(vm => vm.ProfileShortName)
			//   .Mode(MvxBindingMode.OneTime);
			//set.Bind(publicationInfoLabel)
			//	.To(vm => vm.VideoInformationText);
			bindingSet.Bind(videoDescriptionButton).To(vm => vm.VideoDescription)
				.Mode(MvxBindingMode.OneTime);
			//set.Bind(videoImageView)
			//	.For(v => v.ImagePath)
			//	.To(vm => vm.StubImageUrl)
			//	.Mode(MvxBindingMode.OneTime);
			//set.Bind(videoNameLabel)
			//	.To(vm => vm.VideoName)
			//	.Mode(MvxBindingMode.OneTime);
		}

		protected override void SetupControls()
		{
			videoImageView.SetPreviewStyle();
			videoNameLabel.SetMainTitleStyle();

			publicationInfoLabel.SetSmallSubtitleStyle();

			commentatorNameLabel.SetMainTitleStyle();
			commentDateLabel.SetSmallSubtitleStyle();

			profileNameLabel.SetMainTitleStyle();

			likeLabel.SetSmallTitleStyle();
			commentLabel.SetSmallTitleStyle();
			shareLabel.SetSmallTitleStyle();
			shareLabel.Text = Resources.Share;
		}
	}
}

