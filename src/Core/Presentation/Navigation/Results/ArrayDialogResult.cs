using System;
namespace PrankChat.Mobile.Core.Presentation.Navigation.Results
{
    public class ArrayDialogResult
    {
        public string SelectedItem { get; }

        public ArrayDialogResult(string selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }
}
