using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void ThrowIfNull(this object obj, [CallerMemberName] string memberName = "")
        {
            if (obj != null)
            {
                return;
            }

            throw new ArgumentNullException($"Value is null for {memberName}");
        }

        public static T DisposeWith<T>(this T item, CompositeDisposable disposables) where T : IDisposable
        {
            disposables.ThrowIfNull();
            disposables.Add(item);
            return item;
        }

        public static IDisposable SubscribeToEvent<TSource>(this TSource source, EventHandler eventHandler, Action<TSource, EventHandler> addHandler, Action<TSource, EventHandler> removeHandler)
        {
            return Observable.FromEventPattern(handler => addHandler.Invoke(source, handler), handler => removeHandler.Invoke(source, handler))
                             .Subscribe(data => eventHandler.Invoke(data.Sender, (EventArgs)data.EventArgs));
        }

        public static IDisposable SubscribeToEvent<TSource, TEventArgs>(this TSource source, EventHandler<TEventArgs> eventHandler, Action<TSource, EventHandler<TEventArgs>> addHandler, Action<TSource, EventHandler<TEventArgs>> removeHandler)
        {
            return Observable.FromEventPattern<TEventArgs>(handler => addHandler.Invoke(source, handler), handler => removeHandler.Invoke(source, handler))
                             .Subscribe(data => eventHandler.Invoke(data.Sender, data.EventArgs));
        }

        public static IDisposable SubscribeToCollectionChanged<TSource>(this TSource source, NotifyCollectionChangedEventHandler eventHandler) where TSource : INotifyCollectionChanged
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(handler => source.CollectionChanged += handler, handler => source.CollectionChanged -= handler)
                             .Subscribe(data => eventHandler.Invoke(data.Sender, data.EventArgs));
        }

        public static IDisposable SubscribeToPropertyChanged<TSource>(this TSource source, string propertyName, Action handleAction) where TSource : INotifyPropertyChanged
        {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(handler => source.PropertyChanged += handler, handler => source.PropertyChanged -= handler)
                             .Where(data => data.EventArgs.PropertyName == propertyName)
                             .Subscribe(data => handleAction.Invoke());
        }
    }
}
