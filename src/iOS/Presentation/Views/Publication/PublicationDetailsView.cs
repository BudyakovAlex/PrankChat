using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{

    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class PublicationDetailsView : BaseGradientBarView<PublicationDetailsViewModel>
    {
        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<PublicationDetailsView, PublicationDetailsViewModel>();

            set.Bind(commentatorNameLabel)
                .To(vm => vm.CommentatorName);

            set.Bind(commentatorPhotoImageView)
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(commentatorPhotoImageView)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(commentatorPhotoImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.CommentatorPhotoUrl)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(commentLabel)
                .To(vm => vm.NumberOfCommentText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(commentView.Tap())
                .For(v => v.Command)
                .To(vm => vm.OpenCommentsCommand)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(commentDateLabel)
                .To(vm => vm.CommentDateText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(likeLabel)
                .To(vm => vm.NumberOfLikesText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileNameLabel)
                .To(vm => vm.ProfileName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profilePhotoImageView)
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profilePhotoImageView)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profilePhotoImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.ProfilePhotoUrl)
                .WithConversion<PlaceholderColorConverter>()
                .Mode(MvxBindingMode.OneTime);

            set.Bind(publicationInfoLabel)
                .To(vm => vm.VideoInformationText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(videoDescriptionButton)
                .To(vm => vm.VideoDescription)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(videoImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.VideoUrl)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(videoNameLabel)
                .To(vm => vm.VideoName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileShortNameLabel)
                .To(vm => vm.ProfileShortName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileShortNameLabel)
                .For(v => v.BindHidden())
                .To(vm => vm.ProfilePhotoUrl)
                .Mode(MvxBindingMode.OneTime);

            set.Apply();
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

