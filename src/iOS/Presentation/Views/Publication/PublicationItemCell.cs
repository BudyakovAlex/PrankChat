using CoreAnimation;
using CoreGraphics;
using FFImageLoading.Cross;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public partial class PublicationItemCell : BaseVideoTableCell<PublicationItemCell, PublicationItemViewModel>
    {
        protected PublicationItemCell(IntPtr handle)
            : base(handle)
        {
        }

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set
            {
                _isLiked = value;
                if (_isLiked)
                {
                    var image = UIImage.FromBundle("ic_like").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    likeButton.SetImage(image, UIControlState.Normal);
                    likeButton.TintColor = Theme.Color.Accent;
                }
                else
                {
                    var image = UIImage.FromBundle("ic_like_hollow");
                    likeButton.SetImage(image, UIControlState.Normal);
                    likeButton.TintColor = null;
                }
            }
        }

        private bool _isDisliked;
        public bool IsDisliked
        {
            get => _isDisliked;
            set
            {
                _isDisliked = value;
                if (_isDisliked)
                {
                    var image = UIImage.FromBundle("ic_dislike").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    dislikeButton.SetImage(image, UIControlState.Normal);
                    dislikeButton.TintColor = Theme.Color.Accent;
                }
                else
                {
                    var image = UIImage.FromBundle("ic_dislike_hollow");
                    dislikeButton.SetImage(image, UIControlState.Normal);
                    dislikeButton.TintColor = null;
                }
            }
        }

        public override MvxCachedImageView StubImageView => stubImageView;
        public override UIActivityIndicatorView LoadingActivityIndicator => loadingActivityIndicator;
        protected override UIView VideoView => videoView;
        protected override UIView RootProcessingBackgroundView => placeProcessingOverlay;
        protected override UIView ProcessingBackgroundView => processingBackgroundView;
        protected override UIActivityIndicatorView ProcessingActivityIndicator => processingIndicatorView;
        protected override UILabel ProcessingLabel => processingLabel;

        protected override void SetupControls()
        {
            base.SetupControls();

            competitionBorderView.Layer.CornerRadius = 23;
            videoView.SetPreviewStyle();
            profileNameLabel.SetMainTitleStyle();
            publicationInfoLabel.SetSmallSubtitleStyle();
            videoNameLabel.SetTitleStyle();
            likeLabel.SetSmallTitleStyle();
            dislikeLabel.SetSmallTitleStyle();
            commentsLabel.SetSmallTitleStyle();

            // TODO: Unhide this button when video saving will be available.
            bookmarkButton.Hidden = true;

            var gradient = new CAGradientLayer
            {
                CornerRadius = 10,
                MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner,
                StartPoint = new CGPoint(0f, 1f),
                EndPoint = new CGPoint(1f, 1f),
                Colors = new[] { Theme.Color.CompetitionPhaseVotingSecondary.CGColor, Theme.Color.CompetitionPhaseVotingPrimary.CGColor }
            };

            placeProcessingOverlay.Layer.InsertSublayer(gradient, 0);
            placeProcessingOverlay.Layer.CornerRadius = 10;
            processingBackgroundView.Layer.CornerRadius = 8;
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<PublicationItemCell, PublicationItemViewModel>();

            bindingSet.Bind(this).For(v => v.IsLiked).To(vm => vm.IsLiked);
            bindingSet.Bind(this).For(v => v.IsDisliked).To(vm => vm.IsDisliked);

            bindingSet.Bind(profileImage.Tap()).For(v => v.Command).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(profileImage).For(v => v.ImagePath).To(vm => vm.AvatarUrl);

            bindingSet.Bind(commentsLabel).For(v => v.Text).To(vm => vm.NumberOfCommentsPresentation);
            bindingSet.Bind(commentButton).For(v => v.BindTap()).To(vm => vm.ShowCommentsCommand);
            bindingSet.Bind(commentsLabel).For(v => v.BindTap()).To(vm => vm.ShowCommentsCommand);
            bindingSet.Bind(commentsLabel).For(v => v.Text).To(vm => vm.NumberOfCommentsPresentation);

            bindingSet.Bind(profileImage).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(profileNameLabel).To(vm => vm.ProfileName);
            bindingSet.Bind(videoNameLabel).To(vm => vm.VideoName);
            bindingSet.Bind(publicationInfoLabel).To(vm => vm.VideoInformationText);
            bindingSet.Bind(likeButton).To(vm => vm.LikeCommand);
            bindingSet.Bind(likeLabel.Tap()).For(v => v.Command).To(vm => vm.LikeCommand);
            bindingSet.Bind(dislikeButton).To(vm => vm.DislikeCommand);
            bindingSet.Bind(dislikeLabel.Tap()).For(v => v.Command).To(vm => vm.DislikeCommand);
            bindingSet.Bind(bookmarkButton.Tap()).For(v => v.Command).To(vm => vm.BookmarkCommand);

            bindingSet.Bind(likeLabel).To(vm => vm.NumberOfLikesText).Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(dislikeLabel).To(vm => vm.NumberOfDislikesText).Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(moreButton).To(vm => vm.OpenSettingsCommand).Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(shareButton).To(vm => vm.ShareCommand).Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(videoView).For(v => v.BindTap()).To(vm => vm.ShowFullScreenVideoCommand);
            bindingSet.Bind(placeProcessingOverlay).For(v => v.BindVisible()).To(vm => vm.IsVideoProcessing);
            bindingSet.Bind(videoView).For(v => v.BindHidden()).To(vm => vm.IsVideoProcessing);
            bindingSet.Bind(this).For(v => v.CanShowStub).To(vm => vm.IsVideoProcessing).WithConversion<MvxInvertedBooleanConverter>();

            bindingSet.Bind(competitionBorderView).For(v => v.BindVisible()).To(vm => vm.IsCompetitionVideo);
            bindingSet.Bind(competitionCupImageView).For(v => v.BindVisible()).To(vm => vm.IsCompetitionVideo);
        }
    }
}
