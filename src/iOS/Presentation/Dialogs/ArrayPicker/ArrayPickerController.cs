using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;

namespace PrankChat.Mobile.iOS.Presentation.Dialogs.ArrayPicker
{
    public partial class ArrayPickerController : BaseDialog<ArrayDialogViewModel>
    {
        private MvxPickerViewModel _pickerViewModel;

        protected override void Bind()
        {
            var setBind = this.CreateBindingSet<ArrayPickerController, ArrayDialogViewModel>();

            setBind.Bind(_pickerViewModel)
                .For(p => p.SelectedItem)
                .To(vm => vm.SelectedItem);

            setBind.Bind(_pickerViewModel)
                .For(p => p.ItemsSource)
                .To(vm => vm.Items);

            setBind.Bind(doneButton)
                .To(vm => vm.DoneCommand);

            setBind.Bind(cancelButton)
                .To(vm => vm.CloseCommand);

            setBind.Apply();
        }

        protected override void SetupControls()
        {
            _pickerViewModel = new MvxPickerViewModel(arrayPickerView);
            arrayPickerView.Model = _pickerViewModel;
            arrayPickerView.ShowSelectionIndicator = true;

            doneButton.Title = Resources.Select;
            cancelButton.Title = Resources.Cancel;
        }
    }
}

