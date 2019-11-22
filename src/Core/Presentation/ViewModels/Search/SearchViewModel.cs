using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private string _searchValue;

        public SearchViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public MvxObservableCollection<ProfileSearchItemViewModel> Items { get; } = new MvxObservableCollection<ProfileSearchItemViewModel>();

        public string SearchValue
        {
            get => _searchValue;
            set
            {
                if (SetProperty(ref _searchValue, value))
                {
                    SearchCommand.Execute(value);
                }
            }
        }

        public ICommand SearchCommand => new MvxAsyncCommand<string>(OnSearchCommand);

        private Task OnSearchCommand(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return Task.CompletedTask;
            }

            Items.Add(new ProfileSearchItemViewModel("@AnnaGaretta", "Мой профиль - моя крепость"));
            Items.Add(new ProfileSearchItemViewModel("@UniverseDestroyer4328", "Меня сложно найти, легко потерять и невозможно забыть."));
            Items.Add(new ProfileSearchItemViewModel("@4234lex", "Меломан, милиционер, филантроп."));

            return Task.CompletedTask;
        }
    }
}
