using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FlightQuoteCleaner.Google;

namespace FlightQuoteCleaner
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DataAccessWebsite>().As<IDataAccess>();
            builder.RegisterType<QuoteCleaner>().As<IQuoteCleaner>();
            builder.RegisterType<SpreadSheetUpdater>().As<ISpreadSheetUpdater>();
            var container = builder.Build();

            List<object> quoteList;

            IDataAccess textFileHandler;
            IQuoteCleaner quoteCleaner;
            ISpreadSheetUpdater spreadSheetUpdater;

            using (var scope = container.BeginLifetimeScope())
            {
                textFileHandler = scope.Resolve<IDataAccess>();
                quoteCleaner = scope.Resolve<IQuoteCleaner>();
                spreadSheetUpdater = scope.Resolve<ISpreadSheetUpdater>();
            }

            var dirtyQuotes = textFileHandler.GetQuoteString();
            var quotes = quoteCleaner.FilterPrices(dirtyQuotes);
            quoteList = quoteCleaner.GenerateQuoteObjectList(quotes);

            textFileHandler.WriteCleanQuotes(quotes);

            spreadSheetUpdater.Quotes = quoteList;
            spreadSheetUpdater.UpdateSpreadSheetData();
        }
    }
}
