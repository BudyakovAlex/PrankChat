﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items
{
    public class RatingItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigatiobService;
        private DateTime _orderTime;

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => _orderTime.ToString("dd : hh : mm");

        public string PriceText { get; }

        public List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

        public double DownsampleWidth { get; } = 100;

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public RatingItemViewModel(INavigationService navigatiobService,
                                  string orderTitle,
                                  string profilePhotoUrl,
                                  string priceText,
                                  DateTime time)
        {
            _navigatiobService = navigatiobService;
            OrderTitle = orderTitle;
            ProfilePhotoUrl = profilePhotoUrl;
            PriceText = priceText;
            _orderTime = time;
        }

        private Task OnOpenDetailsOrderAsync()
        {
            return _navigatiobService.ShowDetailsOrderView();
        }
    }
}
