using System;
using Android.Content;

namespace PrankChat.Mobile.Droid.Decorators
{
    public class BottomSpaceDecorator : DecoratorBase
    {
        private readonly int _spaceInPx;
        private readonly bool _isOnlyLast;

        public BottomSpaceDecorator(
            Context context,
            int spaceInPx,
            bool isOnlyLast = false) : base(context)
        {
            _spaceInPx = spaceInPx;
            _isOnlyLast = isOnlyLast;

            Initialize(context);
        }

        public BottomSpaceDecorator(Context context) : this(context, 0)
        {
        }

        protected override void Initialize(Android.Content.Res.Resources resources)
        {
            base.Initialize(resources);

            CardBottom = _isOnlyLast ? 0 : _spaceInPx;
            CardLastBottom = _spaceInPx;
        }
    }
}
