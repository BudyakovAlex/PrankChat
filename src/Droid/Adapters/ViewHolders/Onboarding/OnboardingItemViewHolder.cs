using System;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Onboarding.Items;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Adapters.ViewHolders.Onboarding
{
    public class OnboardingItemViewHolder : CardViewHolder<OnboardingItemViewModel>
    {
        private ImageView _imageView;
        private TextView _titleTextView;
        private TextView _descriptionTextView;

        public OnboardingItemViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        private OnBoardingPageType _type;
        public OnBoardingPageType Type
        {
            get => _type;
            set
            {
                _type = value;

                var resourceId = GetImageResourceId(_type);
                _imageView.SetImageResource(resourceId);
            }
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _imageView = view.FindViewById<ImageView>(Resource.Id.image_view);
            _titleTextView = view.FindViewById<TextView>(Resource.Id.title_text_view);
            _descriptionTextView = view.FindViewById<TextView>(Resource.Id.description_text_view);
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<OnboardingItemViewHolder, OnboardingItemViewModel>();

            bindingSet.Bind(this).For(v => v.Type).To(vm => vm.Type);
            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_descriptionTextView).For(v => v.Text).To(vm => vm.Description);
        }

        private int GetImageResourceId(OnBoardingPageType type)
        {
            switch (type)
            {
                case OnBoardingPageType.FirstSlide:
                    return Resource.Drawable.bg_onboarding_first_slide;
                case OnBoardingPageType.SecondSlide:
                    return Resource.Drawable.bg_onboarding_second_slide;
                case OnBoardingPageType.ThirdSlide:
                    return Resource.Drawable.bg_onboarding_third_slide;
                case OnBoardingPageType.FourthSlide:
                    return Resource.Drawable.bg_onboarding_fourth_slide;
                case OnBoardingPageType.FifthSlide:
                    return Resource.Drawable.bg_onboarding_first_slide;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
