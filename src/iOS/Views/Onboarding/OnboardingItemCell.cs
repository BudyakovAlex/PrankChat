using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Onboarding.Items;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Onboarding
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

        private string GetImageName(OnBoardingPageType type) => type switch
        {
            OnBoardingPageType.FirstSlide => ImageNames.BackgroundOnboardingFirstSlide,
            OnBoardingPageType.SecondSlide => ImageNames.BackgroundOnboardingSecondSlide,
            OnBoardingPageType.ThirdSlide => ImageNames.BackgroundOnboardingThirdSlide,
            OnBoardingPageType.FourthSlide => ImageNames.BackgroundOnboardingFourthSlide,
            OnBoardingPageType.FifthSlide => ImageNames.BackgroundOnboardingFifthSlide,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
