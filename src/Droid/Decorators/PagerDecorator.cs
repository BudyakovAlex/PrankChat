using System;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;

namespace PrankChat.Mobile.Droid.Decorators
{
    public class PagerDecorator : DecoratorBase
    {
        public PagerDecorator(Context context)
            : base(context)
        {
        }

        protected int CardFirstLeft { get; set; }

        protected int CardLastRight { get; set; } 

        protected override void DoInit(Android.Content.Res.Resources resources)
        {
            base.DoInit(resources);

            CardFirstLeft = resources.GetDimensionPixelSize(Resource.Dimension.card_decoration_horizontal_left_first);
            CardLastRight = resources.GetDimensionPixelSize(Resource.Dimension.card_decoration_horizontal_right_last);

            CardLeft = resources.GetDimensionPixelSize(Resource.Dimension.card_decoration_horizontal_left);
            CardTop = resources.GetDimensionPixelSize(Resource.Dimension.card_decoration_horizontal_top);
            CardRight = resources.GetDimensionPixelSize(Resource.Dimension.card_decoration_horizontal_right);
            CardBottom = resources.GetDimensionPixelSize(Resource.Dimension.card_decoration_horizontal_bottom);
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            base.GetItemOffsets(outRect, view, parent, state);

            bool isFirst;
            bool isLast;
            GetElementPosition(parent, view, out isFirst, out isLast);

            if (isFirst)
            {
                outRect.Left = CardFirstLeft;
            }

            if (isLast)
            {
                outRect.Right = CardLastRight;
            }
        }
    }
}