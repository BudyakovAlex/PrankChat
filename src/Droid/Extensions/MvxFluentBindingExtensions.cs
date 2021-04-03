using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using System;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class MvxFluentBindingExtensions
    {
        public static MvxFluentBindingDescription<TTarget> WithConversion<TTarget, TSource, TResult>(this MvxFluentBindingDescription<TTarget> description, Func<TSource, TResult> convertFunc)
            where TTarget : class =>
            description.WithConversion(new DelegateConverter<TSource, TResult>(convertFunc), null);

        public static MvxFluentBindingDescription<TTarget, TViewModel> WithConversion<TTarget, TViewModel, TSource, TResult>(this MvxFluentBindingDescription<TTarget, TViewModel> description, Func<TSource, TResult> convertFunc)
            where TTarget : class =>
            description.WithConversion(new DelegateConverter<TSource, TResult>(convertFunc), null);
    }
}