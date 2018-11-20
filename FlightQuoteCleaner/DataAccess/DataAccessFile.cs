using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FlightQuoteCleaner
{
    class DataAccessFile : IDataAccess
    {
        string _fileReadLocation;
        string _fileWriteLocation;
        public DataAccessFile()
        {
            _fileReadLocation = ConfigurationManager.AppSettings["ReadLocation"];
            _fileWriteLocation = ConfigurationManager.AppSettings["WriteLocation"];
        }

        public string GetQuoteString()
        {
            string quotes;
            using (var streamReader = new StreamReader(_fileReadLocation))
            {
                quotes = streamReader.ReadToEnd();
            }
            return quotes;
        }

        public void WriteCleanQuotes(string quotes)
        {
            File.WriteAllText(_fileWriteLocation, quotes);
        }
    }
}
