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

            IDataAccess dataAccessHandler;
            IQuoteCleaner quoteCleaner;
            ISpreadSheetUpdater spreadSheetUpdater;

            using (var scope = container.BeginLifetimeScope())
            {
                dataAccessHandler = scope.Resolve<IDataAccess>();
                quoteCleaner = scope.Resolve<IQuoteCleaner>();
                spreadSheetUpdater = scope.Resolve<ISpreadSheetUpdater>();
            }

            var dirtyQuotes = dataAccessHandler.GetQuoteString();
            var quotes = quoteCleaner.FilterPrices(dirtyQuotes);
            quoteList = quoteCleaner.GenerateQuoteObjectList(quotes);

            dataAccessHandler.WriteCleanQuotes(quotes);

            spreadSheetUpdater.Quotes = quoteList;
            spreadSheetUpdater.UpdateSpreadSheetData();
            Console.ReadLine();
        }
    }
}
