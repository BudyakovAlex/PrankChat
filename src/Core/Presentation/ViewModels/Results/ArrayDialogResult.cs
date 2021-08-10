namespace PrankChat.Mobile.Core.Presentation.ViewModels.Results
{
    public class ArrayDialogResult
    {
        public ArrayDialogResult(string selectedItem)
        {
            SelectedItem = selectedItem;
        }

        public string SelectedItem { get; }
    }
}