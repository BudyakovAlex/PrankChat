﻿using System;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.Core.Presentation.Navigation
{
    public interface INavigationService
    {
        Task AppStart();

        Task ShowLoginView();

        Task ShowPasswordRecoveryView();

        Task ShowRegistrationView();

        Task ShowRegistrationSecondStepView();

        Task ShowRegistrationThirdStepView();

        Task ShowMainView();

        Task ShowMainViewContent();

        Task ShowCommentsView();

        Task ShowNotificationView();

        Task<bool> CloseView(BaseViewModel viewModel);
    }
}
