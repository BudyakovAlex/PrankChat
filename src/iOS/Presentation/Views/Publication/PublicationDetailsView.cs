﻿using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    [MvxModalPresentation(WrapInNavigationController = true)]
	public partial class PublicationDetailsView : BaseGradientBarView<PublicationDetailsViewModel>
	{
		protected override void Bind()
		{
			var setBind = this.CreateBindingSet<PublicationDetailsView, PublicationDetailsViewModel>();

			setBind.Bind(commentatorNameLabel)
				.To(vm => vm.CommentatorName);

			setBind.Bind(commentatorPhotoImageView)
				.For(v => v.ImagePath)
				.To(vm => vm.CommentatorPhotoUrl)
				.Mode(MvxBindingMode.OneTime);

			//TODO: uncomment when vm will be ready
			//set.Bind(commentatorPhotoImageView)
			//   .For(v => v.BindTap())
			//   .To(vm => vm.OpenUserProfileCommand);

			setBind.Bind(commentLabel)
				.To(vm => vm.NumberOfCommentText)
				.Mode(MvxBindingMode.OneTime);

			setBind.Bind(commentView.Tap())
				.For(v => v.Command)
				.To(vm => vm.OpenCommentsCommand)
				.Mode(MvxBindingMode.OneTime);

			setBind.Bind(commentDateLabel)
				.To(vm => vm.CommentDateText)
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

			setBind.Bind(videoDescriptionButton)
				.To(vm => vm.VideoDescription)
				.Mode(MvxBindingMode.OneTime);

			//set.Bind(videoImageView)
			//	.For(v => v.ImagePath)
			//	.To(vm => vm.StubImageUrl)
			//	.Mode(MvxBindingMode.OneTime);

			//set.Bind(videoNameLabel)
			//	.To(vm => vm.VideoName)
			//	.Mode(MvxBindingMode.OneTime);

			setBind.Apply();
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

