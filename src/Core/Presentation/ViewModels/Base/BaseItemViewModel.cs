using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public abstract class BaseItemViewModel : MvxNotifyPropertyChanged
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
    }
}
