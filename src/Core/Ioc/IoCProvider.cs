using MvvmCross.IoC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PrankChat.Mobile.Core.Ioc
{
    public class IoCProvider : IIoCProvider
    {
        protected ConcurrentDictionary<Type, HashSet<Type>> SingletonRegistrations { get; } = new ConcurrentDictionary<Type, HashSet<Type>>();

        private IMvxIoCProvider iocProvider { get; }

        public IoCProvider(IMvxIoCProvider iocProvider)
        {
            this.iocProvider = iocProvider;
        }

        public void RegisterManyAsSingleton<TInterface, TType>()
        {
            var targetType = typeof(TInterface);

            var registrations = SingletonRegistrations.GetOrAdd(targetType, type => new HashSet<Type>());
            registrations.Add(typeof(TType));
        }

        public IEnumerable<TInterface> ResolveMany<TInterface>() where TInterface : class
        {
            var targetType = typeof(TInterface);

            var isAnySingletonRegistration = SingletonRegistrations.TryGetValue(targetType, out var singletonTypes);
            if (!isAnySingletonRegistration)
            {
                throw new ArgumentException($"Failed to resolve type {targetType.Name}");
            }

            foreach (var type in singletonTypes)
            {
                yield return IocConstruct<TInterface>(type);
            }
        }

        public TObject IocConstruct<TObject>(Type type) where TObject : class
        {
            var constructors = type.GetConstructors();
            var currentCtor = constructors.FirstOrDefault();
            if (currentCtor is null)
            {
                throw new ArgumentException($"Failed to construct type {type.Name}");
            }

            var parameters = currentCtor.GetParameters();

            var parametersInstances = new List<object>();
            foreach (var parameter in parameters)
            {
                parametersInstances.Add(iocProvider.Resolve(parameter.ParameterType));
            }

            var instance = currentCtor.Invoke(parametersInstances.ToArray());
            return (TObject)instance;
        }

        public TObject IocConstruct<TObject>() where TObject : class
        {
            return IocConstruct<TObject>(typeof(TObject));
        }

        public object Resolve(Type type)
        {
            return iocProvider.Resolve(type);
        }

        public TObject Resolve<TObject>() where TObject : class
        {
            return iocProvider.Resolve<TObject>();
        }

        public void RegisterSingleton<TInterface>(TInterface implementation) where TInterface : class
        {
            iocProvider.RegisterSingleton(implementation);
        }

        public void RegisterSingleton(Type interfaceType, object implementation)
        {
            iocProvider.RegisterSingleton(interfaceType, implementation);
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> constructorFunc) where TInterface : class
        {
            iocProvider.LazyConstructAndRegisterSingleton(constructorFunc);
        }

        public void RegisterSingleton(Type interfaceType, Func<object> constructorFunc)
        {
            iocProvider.LazyConstructAndRegisterSingleton(interfaceType, constructorFunc);
        }

        public void RegisterSingleton<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom
        {
            iocProvider.LazyConstructAndRegisterSingleton<TFrom, TTo>();
        }

        public void RegisterType(Type typeFrom, Type typeTo)
        {
            iocProvider.RegisterType(typeFrom, typeTo);
        }

        public void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom
        {
            iocProvider.RegisterType(typeof(TFrom), typeof(TTo));
        }

        public void RegisterType<TObject>() where TObject : class
        {
            iocProvider.RegisterType(() => Resolve<TObject>());
        }

        public bool TryResolve(Type type, out object resolvedObject)
        {
            return iocProvider.TryResolve(type, out resolvedObject);
        }

        public bool TryResolve<TObject>(out TObject resolvedObject) where TObject : class
        {
            return iocProvider.TryResolve(out resolvedObject);
        }

        public void CallbackWhenRegistered<TObject>(Action action) where TObject : class
        {
            iocProvider.CallbackWhenRegistered<TObject>(action);
        }

        public void CallbackWhenRegistered<TObject>(Action<TObject> action) where TObject : class
        {
            iocProvider.CallbackWhenRegistered(action);
        }
    }
}
