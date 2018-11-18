using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightQuoteCleaner
{
    class TextFileHandler : IDataAccess
    {
        string _fileReadLocation;
        string _fileWriteLocation;
        public TextFileHandler()
        {
            _fileReadLocation = @"C:\Users\pedro\OneDrive\Documentos\quotes.txt";
            _fileWriteLocation = @"C:\Users\pedro\OneDrive\Documentos\quotesClean.txt";
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
