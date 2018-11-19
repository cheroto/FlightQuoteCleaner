using System.Collections.Generic;

namespace FlightQuoteCleaner.Google
{
    public interface ISpreadSheetUpdater
    {
        List<object> Quotes { get; set; }

        void UpdateSpreadSheetData();
    }
}