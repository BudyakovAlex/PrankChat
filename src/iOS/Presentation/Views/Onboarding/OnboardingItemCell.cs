using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding.Items;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Onboarding
{
    public partial class OnboardingItemCell : BaseCollectionCell<OnboardingItemCell, OnboardingItemViewModel>
    {
        protected OnboardingItemCell(IntPtr handle)
            : base(handle)
        {
        }

        private OnBoardingPageType _type;
        public OnBoardingPageType Type
        {
            get => _type;
            set
            {
                _type = value;

                var imageName = GetImageName(_type);
                imageView.Image = UIImage.FromBundle(imageName);
            }
        }

        protected override void SetBindings()
        {
            base.SetBindings();

            using var bindingSet = this.CreateBindingSet<OnboardingItemCell, OnboardingItemViewModel>();

            bindingSet.Bind(this).For(v => v.Type).To(vm => vm.Type);
            bindingSet.Bind(titleLabel).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(descriptionLabel).For(v => v.Text).To(vm => vm.Description);
        }

        private string GetImageName(OnBoardingPageType type)
        {
            switch (type)
            {
                case OnBoardingPageType.FirstSlide:
                    return "bg_onboarding_first_slide";
                case OnBoardingPageType.SecondSlide:
                    return "bg_onboarding_second_slide";
                case OnBoardingPageType.ThirdSlide:
                    return "bg_onboarding_third_slide";
                case OnBoardingPageType.FourthSlide:
                    return "bg_onboarding_fourth_slide";
                case OnBoardingPageType.FifthSlide:
                    return "bg_onboarding_fifth_slide";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
