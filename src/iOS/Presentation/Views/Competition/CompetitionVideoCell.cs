using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionVideoCell : BaseVideoTableCell<CompetitionVideoCell, CompetitionVideoViewModel>
    {
        private bool _isLikeButtonVisible;
        private bool _isLiked;

        private nfloat _likeButtonHeightConstraintConstant;
        private nfloat _likeButtonBottomConstraintConstant;

        protected CompetitionVideoCell(IntPtr handle)
            : base(handle)
        {
        }

        protected override UIView VideoView => videoView;

        protected override UIActivityIndicatorView LoadingActivityIndicator => loadingActivityIndicator;

        protected override UIImageView StubImageView => stubImageView;

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

        public bool IsLiked
        {
            get => _isLiked;
            set
            {
                _isLiked = value;
                if (_isLiked)
                {
                    likeButton.TintColor = Theme.Color.White;
                    likeButton.BackgroundColor = Theme.Color.CompetitionPhaseNewPrimary;
                    likeButton.Alpha = 1f;
                }
                else
                {
                    likeButton.TintColor = Theme.Color.CompetitionPhaseNewPrimary;
                    likeButton.BackgroundColor = UIColor.Clear;
                    likeButton.Alpha = 0.5f;
                }
            }
        }

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

        protected override void SetBindings()
        {
            base.SetBindings();

            var bindingSet = this.CreateBindingSet<CompetitionVideoCell, CompetitionVideoViewModel>();

            //bindingSet.Bind(profileImageView)
            //	.For(v => v.BindTap())
            //	.To(vm => vm.command);

            bindingSet.Bind(profileImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.AvatarUrl);

            //bindingSet.Bind(profileImageView)
            //	.For(v => v.PlaceholderText)
            //	.To(vm => vm.ProfileShortName);

            bindingSet.Bind(profileNameLabel)
                      .For(v => v.Text)
                      .To(vm => vm.UserName);

            bindingSet.Bind(viewsLabel)
                      .For(v => v.Text)
                      .To(vm => vm.ViewsCount);

            bindingSet.Bind(postDateLabel)
                      .For(v => v.Text)
                      .To(vm => vm.PublicationDateString);

            bindingSet.Bind(videoView)
                      .For(v => v.BindTap())
                      .To(vm => vm.ShowFullScreenVideoCommand);

            bindingSet.Bind(stubImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.StubImageUrl);

            bindingSet.Bind(likeButton)
                      .For(v => v.BindTouchUpInside())
                      .To(vm => vm.LikeCommand);

            bindingSet.Bind(likeButton)
                      .For(v => v.BindTitle())
                      .To(vm => vm.LikesCount);

            bindingSet.Bind(this)
                      .For(v => v.IsLiked)
                      .To(vm => vm.IsLiked);

            bindingSet.Bind(this)
                      .For(v => v.IsLikeButtonVisible)
                      .To(vm => vm.CanVoteVideo);

            bindingSet.Apply();
        }

        private void InitializeLikeButton()
        {
            likeButton.Layer.CornerRadius = 4f;
            likeButton.Layer.BorderWidth = 1f;
            likeButton.Layer.BorderColor = Theme.Color.ButtonBorderPrimary.CGColor;

            likeButton.SetImage(UIImage.FromBundle("ic_thumbs_up.png"), UIControlState.Normal);
        }
    }
}
