using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition.Items
{
    public partial class CompetitionItemTableViewCell : BaseTableCell<CompetitionItemTableViewCell, CompetitionItemViewModel>
    {
        private CompetitionView _competitionView;

        protected CompetitionItemTableViewCell(IntPtr handle) : base(handle)
        {
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            BackgroundColor = UIColor.Clear;
            _competitionView = CompetitionView.Create();
            _competitionView.TranslatesAutoresizingMaskIntoConstraints = false;

            AddSubview(_competitionView);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _competitionView.TopAnchor.ConstraintEqualTo(TopAnchor),
                _competitionView.BottomAnchor.ConstraintEqualTo(BottomAnchor),
                _competitionView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _competitionView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor)
            });
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionItemTableViewCell, CompetitionItemViewModel>();

            bindingSet.Bind(_competitionView).For(v => v.DataContext).To(vm => vm);
        }
    }
}