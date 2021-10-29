namespace PrankChat.Mobile.Core.ViewModels.Results
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