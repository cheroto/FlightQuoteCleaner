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
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TextFileHandler>().As<IDataAccess>();
            builder.RegisterType<QuoteCleaner>().As<IQuoteCleaner>();
            var container = builder.Build();

            IDataAccess textFileHandler;
            IQuoteCleaner quoteCleaner;

            using (var scope = container.BeginLifetimeScope())
            {
                textFileHandler = scope.Resolve<IDataAccess>();
                quoteCleaner = scope.Resolve<IQuoteCleaner>();
            }

            var dirtyQuotes = textFileHandler.GetQuoteString();
            var quotes = quoteCleaner.FilterPrices(dirtyQuotes);

            textFileHandler.WriteCleanQuotes(quotes);

            var spreadSheetUpdater = new SpreadSheetUpdater();
            spreadSheetUpdater.PrintSpreadSheetData();
        }
    }
}
