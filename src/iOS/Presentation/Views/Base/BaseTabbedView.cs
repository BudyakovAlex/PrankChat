﻿using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseTabbedView<TViewModel> : BaseGradientBarView<TViewModel>
        where TViewModel : BasePageViewModel
    {
        public BaseTabbedView()
        {
            IsTabbedView = true;
        }
    }
}
