using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.iOS.Views.Base;

namespace PrankChat.Mobile.iOS.Views.Competition.Items
{
    public partial class CompetitionItemTableViewCell : BaseTableCell<CompetitionItemTableViewCell, CompetitionItemViewModel>
    {
        protected CompetitionItemTableViewCell(IntPtr handle) : base(handle)
        {
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionItemTableViewCell, CompetitionItemViewModel>();

            bindingSet.Bind(competitionView).For(v => v.DataContext).To(vm => vm);
        }
    }
}