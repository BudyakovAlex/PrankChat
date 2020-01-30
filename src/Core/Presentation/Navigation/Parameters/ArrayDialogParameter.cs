using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class ArrayDialogParameter
    {
        public string Title { get; }

        public List<string> Items { get; }

        public ArrayDialogParameter(List<string> items, string title = "")
        {
            Items = items;
            Title = title;
        }
    }
}
