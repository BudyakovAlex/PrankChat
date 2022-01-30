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
        public const float CellHeight = 509f;

        private CompetitionView _competitionView;

        protected CompetitionItemTableViewCell(IntPtr handle) : base(handle)
        {
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            BackgroundColor = UIColor.Clear;
            ContentView.BackgroundColor = UIColor.Clear;

            _competitionView = CompetitionView.Create();
            _competitionView.TranslatesAutoresizingMaskIntoConstraints = false;
            _competitionView.Layer.MasksToBounds = false;

            ContentView.AddSubview(_competitionView);
            ContentView.ClipsToBounds = false;
            ContentView.Layer.MasksToBounds = false;
            Layer.MasksToBounds = false;

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _competitionView.TopAnchor.ConstraintEqualTo(TopAnchor, 5),
                _competitionView.BottomAnchor.ConstraintEqualTo(BottomAnchor, -5f),
                _competitionView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 20f),
                _competitionView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -20f)
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