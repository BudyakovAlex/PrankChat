using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public abstract class BaseItemViewModel : MvxNotifyPropertyChanged
    {
        public virtual double DownsampleWidth { get; } = 100;

        public virtual List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
    }
}
