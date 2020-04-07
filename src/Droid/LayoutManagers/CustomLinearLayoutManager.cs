using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;

namespace PrankChat.Mobile.Droid.LayoutManagers
{
    public class CustomLinearLayoutManager : SafeLinearLayoutManager
    {
        private int visibleItemsCount = 1;

        public CustomLinearLayoutManager(Context context, int orientation, bool reverseLayout, int visibleItemsCount)
            : base(context, orientation, reverseLayout)
        {
            VisibleItemsCount = visibleItemsCount;
        }

        public CustomLinearLayoutManager(Context context, int visibleItemsCount)
            : base(context, orientation: Vertical, reverseLayout: false)
        {
            VisibleItemsCount = visibleItemsCount;
        }

        /// <summary>
        /// Offset in pixels
        /// </summary>
        public int Offset { get; set; }

        public int VisibleItemsCount
        {
            get => visibleItemsCount;
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(VisibleItemsCount), $"Must be bigger than 0");
                }

                visibleItemsCount = value;
            }
        }

        public override void MeasureChildWithMargins(View child, int widthUsed, int heightUsed)
        {
            var lpTmp = (RecyclerView.LayoutParams)child.LayoutParameters;
            widthUsed += lpTmp.LeftMargin + lpTmp.RightMargin;
            heightUsed += lpTmp.TopMargin + lpTmp.BottomMargin;

            MeasureChild(child, widthUsed, heightUsed);
        }

        public override void MeasureChild(View child, int widthUsed, int heightUsed)
        {
            // measure base to trigger decoration calculations
            // unforunatelly, function getItemDecorInsetsForChild on RecyclerView object is private
            // but this functions triggers some logic on LayoutParams to have rectangle with calculated decorations
            // which are used later on layout step
            base.MeasureChild(child, widthUsed, heightUsed);

            int childWidth;
            int childHeight;
            var lpTmp = (RecyclerView.LayoutParams)child.LayoutParameters;

            if (Orientation == Horizontal)
            {
                childWidth = GetAdjustedDimen(Width);
                childHeight = lpTmp.Height;
            }
            else
            {
                childWidth = lpTmp.Width;
                childHeight = GetAdjustedDimen(Height);
            }

            var widthSpec =
                View.MeasureSpec.MakeMeasureSpec(childWidth,
                                                 childWidth < 0 ? MeasureSpecMode.AtMost : MeasureSpecMode.Exactly);
            var heightSpec =
                View.MeasureSpec.MakeMeasureSpec(childHeight,
                                                 childHeight < 0 ? MeasureSpecMode.AtMost : MeasureSpecMode.Exactly);

            child.Measure(widthSpec, heightSpec);
        }

        private int GetAdjustedDimen(int initialDime)
        {
            return (initialDime / visibleItemsCount) - Offset;
        }
    }
}