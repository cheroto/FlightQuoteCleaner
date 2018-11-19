using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlightQuoteCleaner
{
    public interface IQuoteCleaner
    {
        string RemovePreviousPrices(string quotes);
        string RemoveTextInBetween(string quotes);
        string RemoveTrailingTextBack(string quotes);
        string RemoveTrailingTextFront(string quotes);
        List<Object> GenerateQuoteObjectList(string quotes);

        string FilterPrices(string quotes);
    }

    public class QuoteCleaner : IQuoteCleaner
    {
        public string FilterPrices(string quotes)
        {
            quotes = RemovePreviousPrices(quotes);
            quotes = RemoveTextInBetween(quotes);
            quotes = RemoveTrailingTextBack(quotes);
            quotes = RemoveTrailingTextFront(quotes);
            return quotes;
        }

        public List<object> GenerateQuoteObjectList(string quotes)
        {
            quotes = quotes
                .Replace("$", "")
                .Replace(",","")
                .Replace("\r\n", ";");
            return quotes.Split(';').ToList<Object>();
        }

        public string RemovePreviousPrices(string quotes)
        {
            Regex rgBothPrices = new Regex(@"\$.*?\r\n\$.*?\r\n");
            Regex rgFirstPrice = new Regex(@"\$.*?\r\n");
            MatchCollection matches = rgBothPrices.Matches(quotes);
            int startIndex;
            int firstPriceLength;
            int lengthRemoved = 0;

            foreach(Match match in matches)
            {
                firstPriceLength = rgFirstPrice.Match(match.Value).Length;
                startIndex = match.Index + firstPriceLength - lengthRemoved;
                quotes = quotes.Remove(startIndex, match.Length - firstPriceLength);
                lengthRemoved += match.Length - firstPriceLength;
            }
            return quotes;
        }

        public string RemoveTextInBetween(string quotes)
        {
            Regex rg = new Regex(@"1 passenger.*?Airline\r\n", RegexOptions.Singleline);
            quotes = rg.Replace(quotes, "");
            return quotes;
        }

        public string RemoveTrailingTextBack(string quotes)
        {
            Regex rg = new Regex(@".*\$.*?\r\n", RegexOptions.Singleline);
            var outputString = quotes.Substring(0, rg.Match(quotes).Length-2);
            return outputString;
        }

        public string RemoveTrailingTextFront(string quotes)
        {
            Regex rg = new Regex(@".*?\$", RegexOptions.Singleline);
            var outputString = rg.Replace(quotes, "$", 1);
            return outputString;
        }
    }
}
