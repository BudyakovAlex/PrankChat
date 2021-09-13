using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Base
{
    public class BaseCollectionCell<TCell, TViewModel> : MvxCollectionViewCell
    {
        private static readonly CGSize DefaultSize = new CGSize(50, 50);

        protected BaseCollectionCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(SetupCell);
        }

        static BaseCollectionCell()
        {
            CellId = typeof(TCell).Name;
            Nib = UINib.FromName(CellId, NSBundle.MainBundle);
        }

        public static string CellId { get; }

        public static UINib Nib { get; }

        public static CGSize EstimatedSize { get; protected set; } = DefaultSize;

        public TViewModel ViewModel { get; private set; }

        public void SetupCell()
        {
            InitializeViewModel();
            SetupControls();
            SetBindings();
        }

        protected virtual void SetupControls()
        {
            BackgroundColor = UIColor.Clear;
            ContentView.BackgroundColor = UIColor.Clear;
        }

        protected virtual void SetBindings()
        {
        }

        private void InitializeViewModel()
        {
            if (BindingContext?.DataContext is TViewModel viewModel)
            {
                ViewModel = viewModel;
            }
            else
            {
                throw new ArgumentNullException($"Binding context doesn't attached to {typeof(TCell).Name}, expected view model type is {typeof(TViewModel).Name}");
            }
        }
    }
}
