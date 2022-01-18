using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Base
{
    public abstract class BaseTableCell : MvxTableViewCell
    {
        protected BaseTableCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(SetupCell);
        }

        protected virtual void SetupControls()
        {
            BackgroundColor = UIColor.Clear;
            ContentView.BackgroundColor = UIColor.Clear;
            SelectionStyle = UITableViewCellSelectionStyle.Gray;
        }

        protected virtual void Bind()
        {
        }

        private void SetupCell()
        {
            SetupControls();
            Bind();
            Subscribe();
        }

        protected virtual void Subscribe()
        {
        }
    }

    public abstract class BaseTableCell<TCell, TViewModel> : BaseTableCell
        where TCell : BaseTableCell
        where TViewModel : class
    {
        protected BaseTableCell(IntPtr handle)
            : base(handle)
        {
        }

        static BaseTableCell()
        {
            CellId = typeof(TCell).Name;
            Nib = UINib.FromName(CellId, NSBundle.MainBundle);
        }

        public static string CellId { get; }

        public static UINib Nib { get; }

        public TViewModel ViewModel => BindingContext.DataContext as TViewModel;
    }
}