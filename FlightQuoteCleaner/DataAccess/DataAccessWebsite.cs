using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlightQuoteCleaner
{
    public class DataAccessWebsite : IDataAccess
    {
        public string GetQuoteString()
        {
            string clipboardText = string.Empty;
            Process.Start("https://www.google.fr/flights#flt=/m/0h7h6..2018-11-19*./m/0h7h6.2018-11-23;c:CAD;e:1;sd:1;t:s");
            Thread.Sleep(8000);
            SendKeys.SendWait("^(a)");
            Thread.Sleep(1000);
            SendKeys.SendWait("^(c)");
            Thread.Sleep(200);
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                clipboardText = Clipboard.GetText(TextDataFormat.Text);
                Console.WriteLine(clipboardText);
                //Console.ReadLine();
                // Do whatever you need to do with clipboardText
            }
            SendKeys.SendWait("^(w)");
            return clipboardText;
        }

        public void WriteCleanQuotes(string quotes)
        {
            Console.WriteLine(quotes);
        }
    }
}
