using System;
using System.Collections.Generic;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Configuration
{
    internal class ApplicationConfiguration : IApplicationConfiguration
    {
        private const string DevAddress = "https://dev.prankchat.store";
        private const string StageAddress = "https://stage.prankchat.store";

        public string BaseAddress { get; }

        public Version ApiVersion { get; }

        public List<PeriodDataModel> Periods { get; }

        public ApplicationConfiguration()
        {
            BaseAddress = StageAddress;
            ApiVersion = new Version(1, 0);

            // TODO: We sould get data from server. It`s hot fix.
            Periods = new List<PeriodDataModel>()
            {
                new PeriodDataModel(1, "1 час"),
                new PeriodDataModel(2, "2 часа"),
                new PeriodDataModel(4, "4 часа"),
                new PeriodDataModel(6, "6 часов"),
                new PeriodDataModel(8, "8 часов"),
                new PeriodDataModel(12, "12 часов"),
                new PeriodDataModel(16, "16 часов"),
                new PeriodDataModel(20, "20 часов"),
                new PeriodDataModel(24, "24 часа"),
                new PeriodDataModel(32, "32 часа"),
                new PeriodDataModel(36, "36 часов"),
                new PeriodDataModel(40, "40 часов"),
                new PeriodDataModel(44, "44 часа"),
                new PeriodDataModel(48, "48 часов"),
            };
        }
    }
}
