using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions.MvvmCross
{
    public static class MvxFluentBindingDescriptionExtensions
    {
        public static MvxFluentBindingDescription<TTarget, TSource> WithBoolToValueConversion<TTarget, TSource, TTo>(
            this MvxFluentBindingDescription<TTarget, TSource> bindingSet,
            TTo trueValue,
            TTo falseValue)
            where TTarget : class
        {
            return bindingSet.WithConversion(new BoolToStateConverter<TTo>(trueValue, falseValue));
        }

        public static MvxFluentBindingDescription<TTarget, TSource> WithBoolInvertionConversion<TTarget, TSource>(this MvxFluentBindingDescription<TTarget, TSource> bindingSet)
            where TTarget : class
        {
            return bindingSet.WithConversion<MvxInvertedBooleanConverter>();
        }
    }
}