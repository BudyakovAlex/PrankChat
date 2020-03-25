using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseTableCell<TCell, TViewModel> : MvxTableViewCell
        where TViewModel : class
    {
        private const int DefaultCellHeight = 40;

        protected BaseTableCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(SetupCell);
        }

        static BaseTableCell()
        {
            CellId = typeof(TCell).Name;
            Nib = UINib.FromName(CellId, NSBundle.MainBundle);
        }

        public static string CellId { get; }

        public static UINib Nib { get; }

        public static int EstimatedHeight { get; protected set; } = DefaultCellHeight;

        public TViewModel ViewModel => BindingContext.DataContext as TViewModel;

        public void SetupCell()
        {
            SetupControls();
            SetBindings();
        }

        protected virtual void SetupControls()
        {
            BackgroundColor = UIColor.Clear;
            SelectionStyle = UITableViewCellSelectionStyle.Gray;
            ContentView.BackgroundColor = UIColor.Clear;
        }

        protected virtual void SetBindings()
        {
        }
    }
}