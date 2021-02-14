using System;
using System.Collections.Generic;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Configuration
{
    internal class ApplicationConfiguration : IApplicationConfiguration
    {
        private const string DevAddress = "https://dev.prankchat.store";
        private const string StageAddress = "https://stage.prankchat.store";
        private const string ProdAddress = "https://api.prank.chat";

        public string BaseAddress { get; }

        public Version ApiVersion { get; }

        public List<Period> Periods { get; }

        public ApplicationConfiguration()
        {
            BaseAddress = StageAddress;
            ApiVersion = new Version(1, 0);

            // TODO: We should get data from server. It`s hot fix.
            Periods = new List<Period>()
            {
                new Period(1, $"1 {Resources.Create_Order_Hour}"),
                new Period(2, $"2 {Resources.Create_Orders_Hours_Singular}"),
                new Period(4, $"4 {Resources.Create_Orders_Hours_Singular}"),
                new Period(6, $"6 {Resources.Create_Orders_Hours_Plural}"),
                new Period(8, $"8 {Resources.Create_Orders_Hours_Plural}"),
                new Period(12, $"12 {Resources.Create_Orders_Hours_Plural}"),
                new Period(16, $"16 {Resources.Create_Orders_Hours_Plural}"),
                new Period(20, $"20 {Resources.Create_Orders_Hours_Plural}"),
                new Period(24, $"24 {Resources.Create_Orders_Hours_Singular}"),
                new Period(32, $"32 {Resources.Create_Orders_Hours_Singular}"),
                new Period(36, $"36 {Resources.Create_Orders_Hours_Plural}"),
                new Period(40, $"40 {Resources.Create_Orders_Hours_Plural}"),
                new Period(44, $"44 {Resources.Create_Orders_Hours_Singular}"),
                new Period(48, $"48 {Resources.Create_Orders_Hours_Plural}"),
            };
        }
    }
}
