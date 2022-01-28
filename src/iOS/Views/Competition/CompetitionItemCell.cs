using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Views.Base;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    public partial class CompetitionItemCell : BaseCollectionCell<CompetitionItemCell, CompetitionItemViewModel>
    {
        private CompetitionView _competitionView;

        protected CompetitionItemCell(IntPtr handle) : base(handle)
        {
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            BackgroundColor = UIColor.Clear;
            ContentView.BackgroundColor = UIColor.Clear;

            _competitionView = CompetitionView.Create();
            _competitionView.TranslatesAutoresizingMaskIntoConstraints = false;
            _competitionView.Layer.MasksToBounds = true;

            ContentView.AddSubview(_competitionView);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _competitionView.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor),
                _competitionView.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor),
                _competitionView.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor),
                _competitionView.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor)
            });
        }

        protected override void SetBindings()
        {
            base.SetBindings();

            using var bindingSet = this.CreateBindingSet<CompetitionItemCell, CompetitionItemViewModel>();

            bindingSet.Bind(_competitionView).For(v => v.DataContext).To(vm => vm);
        }
    }
}