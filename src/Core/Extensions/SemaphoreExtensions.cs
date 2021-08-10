using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class SemaphoreExtensions
    {
        public static async Task WrapAsync(this SemaphoreSlim semaphoreSlim, Action execute)
        {
            try
            {
                if (semaphoreSlim.CurrentCount == 0)
                {
                    await semaphoreSlim.WaitAsync();
                }
                else
                {
                    // NOTE if semaphore wasn't called yet, it is empty
                    // So we have 2 points:
                    // 1. semaphoreSlim.Wait() will be executed immediately without locking the thread
                    // 2. We have can't call await as it will release the UI thread and use is able to do double clicks for example
                    semaphoreSlim.Wait();
                }

                execute.Invoke();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public static async Task WrapAsync(this SemaphoreSlim semaphoreSlim, Func<Task> execute)
        {
            try
            {
                if (semaphoreSlim.CurrentCount == 0)
                {
                    await semaphoreSlim.WaitAsync();
                }
                else
                {
                    // NOTE if semaphore wasn't called yet, it is empty
                    // So we have 2 points:
                    // 1. semaphoreSlim.Wait() will be executed immediately without locking the thread
                    // 2. We have can't call await as it will release the UI thread and use is able to do double clicks for example
                    semaphoreSlim.Wait();
                }

                await execute.Invoke();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public static async Task<T> WrapAsync<T>(this SemaphoreSlim semaphoreSlim, Func<Task<T>> execute)
        {
            try
            {
                if (semaphoreSlim.CurrentCount == 0)
                {
                    await semaphoreSlim.WaitAsync();
                }
                else
                {
                    // NOTE if semaphore wasn't called yet, it is empty
                    // So we have 2 points:
                    // 1. semaphoreSlim.Wait() will be executed immediately without locking the thread
                    // 2. We have can't call await as it will release the UI thread and use is able to do double clicks for example
                    semaphoreSlim.Wait();
                }

                var result = await execute.Invoke();
                return result;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
