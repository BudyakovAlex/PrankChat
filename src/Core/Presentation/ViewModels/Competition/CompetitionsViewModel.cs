using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;

        public CompetitionsViewModel(IMvxMessenger mvxMessenger,
                                     INavigationService navigationService,
                                     IErrorHandleService errorHandleService,
                                     IApiService apiService,
                                     IDialogService dialogService,
                                     ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
        }

        public MvxObservableCollection<CompetitionsSectionViewModel> Items { get; set; } = new MvxObservableCollection<CompetitionsSectionViewModel>();

        public override Task Initialize()
        {
            var competitions = new List<CompetitionDataModel>() {
                new CompetitionDataModel() {
                    Number = "#0001",
                    Title = "Конкурс роботов",
                    Description = "Описание конкурса",
                    LikesCount = 99,
                    PrizePool = 200000000,
                    HtmlContent = "<html><ul><li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li><li>Aliquam tincidunt mauris eu risus.</li><li>Vestibulum auctor dapibus neque.</li</ul></html>",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png",
                    NewTerm = DateTime.UtcNow.AddDays(-5),
                    VoteTerm = DateTime.UtcNow.AddDays(1)
                },
                new CompetitionDataModel() {
                    Number = "#0002",
                    Title = "Конкурс котов",
                    Description = "Описание конкурса",
                    LikesCount = 99,
                    PrizePool = 2000000,
                    HtmlContent = "<html><ul><li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li><li>Aliquam tincidunt mauris eu risus.</li><li>Vestibulum auctor dapibus neque.</li</ul></html>",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png",
                    NewTerm = DateTime.UtcNow.AddDays(5),
                    VoteTerm = DateTime.UtcNow.AddDays(8)
                },
                new CompetitionDataModel() {
                    Number = "#0003",
                    Title = "Конкурс собак",
                    Description = "Описание конкурса",
                    LikesCount = 99,
                    PrizePool = 2000000,
                    HtmlContent = "<html><ul><li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li><li>Aliquam tincidunt mauris eu risus.</li><li>Vestibulum auctor dapibus neque.</li</ul></html>",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png",
                    NewTerm = DateTime.UtcNow.AddDays(-5),
                    VoteTerm = DateTime.UtcNow.AddDays(-3)
                },
                new CompetitionDataModel() {
                    Number = "#0004",
                    Title = "Конкурс мышей",
                    Description = "Описание конкурса",
                    LikesCount = 99,
                    PrizePool = 2000000,
                    HtmlContent = "<html><ul><li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li><li>Aliquam tincidunt mauris eu risus.</li><li>Vestibulum auctor dapibus neque.</li</ul></html>",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png",
                    NewTerm = DateTime.UtcNow.AddDays(-3),
                    VoteTerm = DateTime.UtcNow.AddDays(4)
                },
                new CompetitionDataModel() {
                    Number = "#0005",
                    Title = "Конкурс брокеров",
                    Description = "Описание конкурса",
                    LikesCount = 99,
                    PrizePool = 2000000,
                    HtmlContent = "<html><ul><li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li><li>Aliquam tincidunt mauris eu risus.</li><li>Vestibulum auctor dapibus neque.</li</ul></html>",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png",
                    NewTerm = DateTime.UtcNow.AddDays(3),
                    VoteTerm = DateTime.UtcNow.AddDays(9)
                },
                new CompetitionDataModel() {
                    Number = "#0006",
                    Title = "Конкурс машин",
                    Description = "Описание конкурса",
                    LikesCount = 99,
                    PrizePool = 1000000,
                    HtmlContent = "<html><ul><li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li><li>Aliquam tincidunt mauris eu risus.</li><li>Vestibulum auctor dapibus neque.</li</ul></html>",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png",
                    NewTerm = DateTime.UtcNow.AddDays(-45),
                    VoteTerm = DateTime.UtcNow.AddDays(-3)
                },
            };

            var sections = competitions.GroupBy(competition => competition.GetPhase())
                                       .Select(group => new CompetitionsSectionViewModel(_mvxMessenger, NavigationService, group.Key, group.ToList()))
                                       .ToList();
            Items.SwitchTo(sections);

            return Task.CompletedTask;
        }
    }
}