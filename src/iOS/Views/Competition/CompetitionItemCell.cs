using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.iOS.Views.Base;
using System;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    public partial class CompetitionItemCell : BaseCollectionCell<CompetitionItemCell, CompetitionItemViewModel>
    {
        protected CompetitionItemCell(IntPtr handle) : base(handle)
        {
        }

        protected override void SetBindings()
        {
            base.SetBindings();

            using var bindingSet = this.CreateBindingSet<CompetitionItemCell, CompetitionItemViewModel>();

            bindingSet.Bind(competitionView).For(v => v.DataContext).To(vm => vm);
        }
    }
}