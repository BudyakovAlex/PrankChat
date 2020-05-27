﻿using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract
{
    public abstract class CardViewHolder : MvxRecyclerViewHolder
    {
        public CardViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
            Init(view);
            this.DelayBind(BindData);
        }

        private void Init(View view)
        {
            DoInit(view);
        }

        protected virtual void DoInit(View view)
        {
        }

        public virtual void ClearAnimation()
        {
            ItemView.ClearAnimation();
        }

        public override void OnViewRecycled()
        {
            base.OnViewRecycled();
            ClearAnimation();
        }

        public virtual void BindData()
        {
        }
    }
}
