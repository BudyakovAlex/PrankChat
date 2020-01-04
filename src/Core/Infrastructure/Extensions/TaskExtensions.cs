using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class TaskExtensions
    {
        public static void FireAndForget(this Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            task.ContinueWith(t =>
            {
                var result = default(object);

                if (t.IsFaulted)
                {
                    // Read the Exception property, so it is no longer 'unobserved'.
                    result = t.Exception;
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"#{nameof(TaskExtensions)}#> "
                                                       + $"A forgotten task {task} fails with:\nEXCEPTION {result}");
#endif
                }

                return result;
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
