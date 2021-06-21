namespace PrankChat.Mobile.Core.Presentation.Navigation.Results
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