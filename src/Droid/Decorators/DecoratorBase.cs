using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;

namespace PrankChat.Mobile.Droid.Decorators
{
    public abstract class DecoratorBase : RecyclerView.ItemDecoration
    {
        protected DecoratorBase(Context context)
        {
            Init(context);
        }

        protected int CardLeft { get; set; }

        protected int CardRight { get; set; }

        protected int CardTop { get; set; }

        protected int CardBottom { get; set; }

        protected int CardFirstTop { get; set; }

        protected int CardLastBottom { get; set; }

        protected static int GetElementPosition(RecyclerView recyclerView, View view, out bool isFirst, out bool isLast)
        {
            var elementPosition = GetElementPosition(recyclerView, view);

            var lastElementPosition = GetLastItemPosition(recyclerView);

            isFirst = elementPosition == 0;
            isLast = elementPosition == lastElementPosition;

            return elementPosition;
        }

        protected static int GetElementPosition(RecyclerView recyclerView, View view)
        {
            return recyclerView.GetChildAdapterPosition(view);
        }

        protected static int GetTotalCount(RecyclerView recyclerView)
        {
            return recyclerView.GetAdapter().ItemCount;
        }

        protected static int GetLastItemPosition(RecyclerView recyclerView)
        {
            return GetTotalCount(recyclerView) - 1;
        }

        protected static RecyclerView.ViewHolder GetViewHolder(RecyclerView recyclerView, View view) => recyclerView.GetChildViewHolder(view);

        private void Init(Context context)
        {
            DoInit(context);
        }

        protected virtual void DoInit(Context context)
        {
            DoInit(context.Resources);
        }

        protected virtual void DoInit(Android.Content.Res.Resources resources)
        {
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            base.GetItemOffsets(outRect, view, parent, state);

            GetElementPosition(parent, view, out var isFirst, out var isLast);

            outRect.Left = CardLeft;
            outRect.Right = CardRight;
            outRect.Top = isFirst ? CardFirstTop : CardTop;
            outRect.Bottom = isLast ? CardLastBottom : CardBottom;
        }
    }
}