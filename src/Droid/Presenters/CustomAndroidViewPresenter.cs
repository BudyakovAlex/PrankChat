using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Droid.Presenters.Attributes;

namespace PrankChat.Mobile.Droid.Presenters
{
    public class CustomAndroidViewPresenter : MvxAppCompatViewPresenter
    {
        public CustomAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            AttributeTypesToActionsDictionary.Add(
                typeof(ClearStackActivityPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = ShowActivityAndClearStack,
                    CloseAction = (viewModel, attribute) => CloseActivity(viewModel, (MvxActivityPresentationAttribute)attribute)
                });
        }

        private Task<bool> ShowActivityAndClearStack(Type viewType, IMvxPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var current = CurrentActivity;
            var result = ShowActivity(viewType, (MvxActivityPresentationAttribute)attribute, request);
            current?.FinishAffinity();
      
            return result;
        }
    }
}