using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Dialogs;

namespace PrankChat.Mobile.iOS.Dialogs.ArrayPicker
{
    public partial class ArrayPickerController : BaseDialog<ArrayDialogViewModel>
    {
        private MvxPickerViewModel _pickerViewModel;

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<ArrayPickerController, ArrayDialogViewModel>();

            bindingSet.Bind(_pickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedItem);
            bindingSet.Bind(_pickerViewModel).For(p => p.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(doneButton).To(vm => vm.DoneCommand);
            bindingSet.Bind(cancelButton).To(vm => vm.CloseCommand);
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

