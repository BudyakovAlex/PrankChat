﻿using System;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    public partial class CompetitionVideoCell : BaseVideoTableCell<CompetitionVideoCell, CompetitionVideoViewModel>
    {
        private nfloat _likeButtonHeightConstraintConstant;
        private nfloat _likeButtonBottomConstraintConstant;

        protected CompetitionVideoCell(IntPtr handle)
            : base(handle)
        {
        }

        public override MvxCachedImageView StubImageView => stubImageView;
        public override UIActivityIndicatorView LoadingActivityIndicator => loadingActivityIndicator;
        protected override UIView VideoView => videoView;
        protected override UIView RootProcessingBackgroundView => rootProcessingBackgroundView;
        protected override UIView ProcessingBackgroundView => processingBackgroundView;
        protected override UILabel ProcessingLabel => processingLabel;
        protected override UIActivityIndicatorView ProcessingActivityIndicator => processingIndicatorView;

        private bool _isLikeButtonVisible;
        public bool IsLikeButtonVisible
        {
            get => _isLikeButtonVisible;
            set
            {
                _isLikeButtonVisible = value;
                if (_isLikeButtonVisible)
                {
                    likeButton.Hidden = false;
                    likeButtonHeightConstraint.Constant = _likeButtonHeightConstraintConstant;
                    likeButtonBottomConstraint.Constant = _likeButtonBottomConstraintConstant;
                }
                else
                {
                    likeButton.Hidden = true;
                    likeButtonHeightConstraint.Constant = 0f;
                    likeButtonBottomConstraint.Constant = 0f;
                }
            }
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
                    likeButton.BackgroundColor = Theme.Color.CompetitionPhaseNewPrimary;
                    likeButton.SetTitleColor(Theme.Color.White, UIControlState.Normal);
                    likeButton.SetImage(UIImage.FromBundle(ImageNames.IconThumbsUp).ApplyTintColor(Theme.Color.White), UIControlState.Normal);
                    likeButton.Alpha = 1f;
                }
                else
                {
                    likeButton.BackgroundColor = UIColor.Clear;
                    likeButton.SetTitleColor(Theme.Color.CompetitionPhaseNewPrimary, UIControlState.Normal);
                    likeButton.SetImage(UIImage.FromBundle(ImageNames.IconThumbsUp).ApplyTintColor(Theme.Color.CompetitionPhaseNewPrimary), UIControlState.Normal);
                    likeButton.Alpha = 0.5f;
                }
            }
        }

        protected override void OnVideoViewTap() =>
           ViewModel?.ShowFullScreenVideoCommand.Execute();

        protected override void SetupControls()
        {
            base.SetupControls();

            _likeButtonHeightConstraintConstant = likeButtonHeightConstraint.Constant;
            _likeButtonBottomConstraintConstant = likeButtonBottomConstraint.Constant;

            videoView.SetPreviewStyle();
            profileNameLabel.SetMainTitleStyle();
            viewsLabel.SetSmallSubtitleStyle();
            postDateLabel.SetSmallSubtitleStyle();

            InitializeLikeButton();
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionVideoCell, CompetitionVideoViewModel>();

            bindingSet.Bind(profileImageView).For(v => v.BindTap()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(profileImageView).For(v => v.ImagePath).To(vm => vm.AvatarUrl);
            bindingSet.Bind(profileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(profileNameLabel).For(v => v.Text).To(vm => vm.UserName);
            bindingSet.Bind(viewsLabel).For(v => v.Text).To(vm => vm.ViewsCount);
            bindingSet.Bind(postDateLabel).For(v => v.Text).To(vm => vm.PublicationDateString);
            bindingSet.Bind(videoView).For(v => v.BindTap()).To(vm => vm.ShowFullScreenVideoCommand);
            bindingSet.Bind(likeButton).For(v => v.BindTouchUpInside()).To(vm => vm.LikeCommand);
            bindingSet.Bind(this).For(v => v.IsLiked).To(vm => vm.IsLiked);
            bindingSet.Bind(this).For(v => v.IsLikeButtonVisible).To(vm => vm.CanVoteVideo);
            bindingSet.Bind(likeButton).For(v => v.BindTitle()).To(vm => vm.LikesCount);
            bindingSet.Bind(RootProcessingBackgroundView).For(v => v.BindVisible()).To(vm => vm.IsVideoProcessing);
            bindingSet.Bind(videoView).For(v => v.BindHidden()).To(vm => vm.IsVideoProcessing);
            bindingSet.Bind(this).For(v => v.CanShowStub).To(vm => vm.IsVideoProcessing)
                      .WithConversion<MvxInvertedBooleanConverter>();
        }

        private void InitializeLikeButton()
        {
            likeButton.Layer.CornerRadius = 4f;
            likeButton.Layer.BorderWidth = 1f;
            likeButton.Layer.BorderColor = Theme.Color.ButtonBorderPrimary.CGColor;

            likeButton.SetImage(UIImage.FromBundle(ImageNames.IconThumbsUp), UIControlState.Normal);
        }
    }
}
