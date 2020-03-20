using System;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsViewModel : BaseItemViewModel, IDisposable
    {
        public MvxObservableCollection<CompetitionsSectionViewModel> Items { get; set; } = new MvxObservableCollection<CompetitionsSectionViewModel>();

        public CompetitionsViewModel()
        {
            LoadAsync();
        }

        private async Task LoadAsync()
        {
        }

        public void Dispose()
        {
            foreach (var item in Items)
            {
                item.Dispose();
            }
        }
    }
}
