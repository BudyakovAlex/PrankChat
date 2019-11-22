using System;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Search
{
    public partial class ProfileSearchItemCell : BaseCell<ProfileSearchItemCell, ProfileSearchItemViewModel>
    {
        static ProfileSearchItemCell()
        {
            EstimatedHeight = 56;
        }

        protected ProfileSearchItemCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            innerView.BackgroundColor = UIColor.Clear;

            profileNameLabel.SetSearchResultTitleLabelStyle();
            profileNameLabel.Text = ViewModel.ProfileName;

            profileDescriptionLabel.SetSearchResultDescriptionLabelStyle();
            profileDescriptionLabel.Text = ViewModel.ProfileDescription;
        }

        protected override void SetBindings()
        {
        }
    }
}

