using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    public abstract class MvxGenericView<TView> : MvxView
    {
        public static readonly NSString Key = new NSString(typeof(TView).Name);
        public static readonly UINib Nib;

        static MvxGenericView()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected MvxGenericView(IntPtr handle) : base(handle)
        {
        }

        public static TView Create() => (TView)(object)Nib.Instantiate(null, null)[0];

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            Initialize();
            this.DelayBind(Bind);
            AddUniqueIdentifiers();
            Localize();
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void Bind()
        {
        }

        protected virtual void Localize()
        {
        }

        protected virtual void AddUniqueIdentifiers()
        {
        }
    }
}
