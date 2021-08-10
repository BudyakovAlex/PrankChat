using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Parameters
{
    public class ArrayDialogParameter
    {
        public ArrayDialogParameter(List<string> items, string title = "")
        {
            Items = items;
            Title = title;
        }

        public string Title { get; }

        public List<string> Items { get; }
    }
}