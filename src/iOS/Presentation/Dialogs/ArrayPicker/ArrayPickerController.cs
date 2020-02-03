using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Dialogs.ArrayPicker
{
    [MvxModalPresentation(
        ModalPresentationStyle = UIModalPresentationStyle.BlurOverFullScreen,
        ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve)]
    public partial class ArrayPickerController : BaseView<ArrayDialogViewModel>
    {
        private MvxPickerViewModel _pickerViewModel;

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<ArrayPickerController, ArrayDialogViewModel>();

            set.Bind(_pickerViewModel)
                .For(p => p.SelectedItem)
                .To(vm => vm.SelectdItem);

            set.Bind(_pickerViewModel)
                .For(p => p.ItemsSource)
                .To(vm => vm.Items);

            set.Bind(doneButton)
                .To(vm => vm.SelectItemCommand);

            set.Apply();
        }

        protected override void SetupControls()
        {
            _pickerViewModel = new MvxPickerViewModel(arrayPickerView);
            arrayPickerView.Model = _pickerViewModel;
            arrayPickerView.ShowSelectionIndicator = true;
            doneButton.Title = Resources.Select;
        }
    }
}

