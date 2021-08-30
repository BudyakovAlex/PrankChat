using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding.Items;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Providers;
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
                    return ImageNames.BackgroundOnboardingFirstSlide;
                case OnBoardingPageType.SecondSlide:
                    return ImageNames.BackgroundOnboardingSecondSlide;
                case OnBoardingPageType.ThirdSlide:
                    return ImageNames.BackgroundOnboardingThirdSlide;
                case OnBoardingPageType.FourthSlide:
                    return ImageNames.BackgroundOnboardingFourthSlide;
                case OnBoardingPageType.FifthSlide:
                    return ImageNames.BackgroundOnboardingFifthSlide;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
