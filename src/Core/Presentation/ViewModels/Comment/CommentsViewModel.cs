﻿using System;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Comment
{
    public class CommentsViewModel : BaseViewModel
    {
        public MvxObservableCollection<CommentItemViewModel> Items { get; } = new MvxObservableCollection<CommentItemViewModel>();

        public CommentsViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override Task Initialize()
        {
            Items.Add(new CommentItemViewModel("Name one", "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500", "First comment", new DateTime(2019, 1, 13)));
            Items.Add(new CommentItemViewModel("Name two", "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500", "Second comment", new DateTime(2019, 4, 18)));
            return base.Initialize();
        }
    }
}
