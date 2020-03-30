using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    public partial class CompetitionPrizePoolItemCell : BaseTableCell<CompetitionPrizePoolItemCell, CompetitionPrizePoolItemViewModel>
    {
        public static readonly nfloat Height = 30f;

        protected CompetitionPrizePoolItemCell(IntPtr handle)
            : base(handle)
        {
        }

        protected override void SetBindings()
        {
            base.SetBindings();

            var bindingSet = this.CreateBindingSet<CompetitionPrizePoolItemCell, CompetitionPrizePoolItemViewModel>();

            bindingSet.Bind(positionLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Position);

            bindingSet.Bind(participantLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Participant);

            bindingSet.Bind(ratingLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Rating);

            bindingSet.Apply();
        }
    }
}
