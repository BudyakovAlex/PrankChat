using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class ArrayDialogParameter
    {
        public List<string> Items { get; }

        public ArrayDialogParameter(List<string> items)
        {
            Items = items;
        }
    }
}
