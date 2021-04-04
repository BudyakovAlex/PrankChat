using MvvmCross.Plugin.Messenger;
using System;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class IMvxMessengerExtensions
    {
        public static IDisposable SubscribeWithCondition<TMessage>(this IMvxMessenger mvxMessenger, Func<TMessage, bool> condition, Action<TMessage> action)
            where TMessage : MvxMessage
        {
            return mvxMessenger.Subscribe<TMessage>(message =>
            {
                if (!condition?.Invoke(message) ?? true)
                {
                    return;
                }

                action?.Invoke(message);
            });
        }
    }
}