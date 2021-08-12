using PrankChat.Mobile.Core.Extensions;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Wrappers
{
    public sealed class SafeExecutionWrapper
    {
        private readonly Func<Exception, Task> _defaultHandleFunc;

        public SafeExecutionWrapper(Func<Exception, Task> defaultHandleFunc = null)
        {
            _defaultHandleFunc = defaultHandleFunc ?? new Func<Exception, Task>((e) => { return Task.CompletedTask; });
        }

        public void Wrap(Action executionAction, Action<Exception> handlerAction = null)
        {
            executionAction.ThrowIfNull();

            try
            {
                executionAction.Invoke();
            }
            catch (Exception ex)
            {
                if (handlerAction is null)
                {
                    _defaultHandleFunc.Invoke(ex);
                    return;
                }

                handlerAction.Invoke(ex);
            }
        }

        public async Task WrapAsync(Func<Task> executionAction, Func<Exception, Task> handlerAction = null)
        {
            executionAction.ThrowIfNull();

            try
            {
                await executionAction.Invoke();
            }
            catch (Exception ex)
            {
                var handleTask = handlerAction?.Invoke(ex) ?? _defaultHandleFunc.Invoke(ex);
                await handleTask;
            }
        }

        public async Task<T> WrapAsync<T>(Func<Task<T>> executionAction, Func<Exception, Task> handlerAction = null)
        {
            executionAction.ThrowIfNull();

            try
            {
                return await executionAction.Invoke();
            }
            catch (Exception ex)
            {
                await (handlerAction?.Invoke(ex) ?? _defaultHandleFunc.Invoke(ex));
                return default;
            }
        }
    }
}