using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Ioc
{
    public interface IIoCProvider
    {
        void RegisterSingleton<TObject>(TObject implementation) where TObject : class;

        void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterType(Type typeFrom, Type typeTo);

        void RegisterType<TObject>() where TObject : class;

        void RegisterSingleton<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterSingleton(Type interfaceType, object implementation);

        void RegisterSingleton<TInterface>(Func<TInterface> constructorFunc)
            where TInterface : class;

        void RegisterSingleton(Type interfaceType, Func<object> constructorFunc);

        void RegisterManyAsSingleton<TInterface, TType>();

        IEnumerable<TInterface> ResolveMany<TInterface>() where TInterface : class;

        TObject IocConstruct<TObject>(Type type) where TObject : class;

        TObject IocConstruct<TObject>() where TObject : class;

        object Resolve(Type type);

        TObject Resolve<TObject>() where TObject : class;

        bool TryResolve(Type type, out object resolvedObject);

        bool TryResolve<TObject>(out TObject resolvedObject) where TObject : class;

        void CallbackWhenRegistered<TObject>(Action action) where TObject : class;

        void CallbackWhenRegistered<TObject>(Action<TObject> action) where TObject : class;
    }
}
