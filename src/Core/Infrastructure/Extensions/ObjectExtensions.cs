using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
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

        public static IDisposable SubscribeToEvent<TSource>(this TSource source, EventHandler handler, Action<TSource, EventHandler> addHandler, Action<TSource, EventHandler> removeHandler)
        {
            return Observable.FromEventPattern(handler => addHandler.Invoke(source, handler), handler => removeHandler.Invoke(source, handler))
                             .Subscribe(data => handler.Invoke(data.Sender, (EventArgs)data.EventArgs));
        }

        public static IDisposable SubscribeToEvent<TSource, TEventArgs>(this TSource source, EventHandler<TEventArgs> handler, Action<TSource, EventHandler<TEventArgs>> addHandler, Action<TSource, EventHandler<TEventArgs>> removeHandler)
        {
            return Observable.FromEventPattern<TEventArgs>(handler => addHandler.Invoke(source, handler), handler => removeHandler.Invoke(source, handler))
                             .Subscribe(data => handler.Invoke(data.Sender, data.EventArgs));
        }

        public static IDisposable SubscribeToCollectionChanged<TSource>(this TSource source, NotifyCollectionChangedEventHandler handler) where TSource : INotifyCollectionChanged
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(handler => source.CollectionChanged += handler, handler => source.CollectionChanged -= handler)
                             .Subscribe(data => handler.Invoke(data.Sender, data.EventArgs));
        }

        public static IDisposable SubscribeToPropertyChanged<TSource>(this TSource source, string propertyName, Action handler) where TSource : INotifyPropertyChanged
        {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(handler => source.PropertyChanged += handler, handler => source.PropertyChanged -= handler)
                             .Where(data => data.EventArgs.PropertyName == propertyName)
                             .Subscribe(data => handler.Invoke());
        }
    }
}
