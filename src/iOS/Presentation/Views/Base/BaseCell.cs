using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseCell<TCell, TViewModel> : MvxTableViewCell
    {
        protected BaseCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(SetupCell);
        }

        static BaseCell()
        {
            CellId = typeof(TCell).Name;
            Nib = UINib.FromName(CellId, NSBundle.MainBundle);
        }

        public static string CellId { get; }

        public static UINib Nib { get; }

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
            SelectionStyle = UITableViewCellSelectionStyle.None;
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
