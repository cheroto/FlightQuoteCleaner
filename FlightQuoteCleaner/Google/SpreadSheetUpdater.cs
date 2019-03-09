using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FlightQuoteCleaner.Google
{
    public class SpreadSheetUpdater : ISpreadSheetUpdater
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public List<Object> Quotes { get; set; }

        public SpreadSheetUpdater()
        {

        }

        public SpreadSheetUpdater(List<object> quotes)
        {
            Quotes = quotes;
        }

        public void UpdateSpreadSheetData()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("Google/credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1BODs_1wmA62KoZUN0dalOCTIPQpvy4YaxffSKY07I2A";
            ValueRange vr = new ValueRange();
            vr.Range = "Data!B24";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, vr.Range);
            ValueRange response = request.Execute();
            string cell = (string)response.Values[0][0];

            String range = "Data!" + cell;
            vr.MajorDimension = "COLUMNS";
            vr.Range = range;
            Quotes.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            vr.Values = new List<IList<Object>>() { Quotes };

            SpreadsheetsResource.ValuesResource.AppendRequest update =
                    service.Spreadsheets.Values.Append(vr, spreadsheetId, range);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            update.Execute();


            vr.Range = "Flight Quotes!I31";
            var time = new List<object>() { DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") };
            vr.Values = new List<IList<Object>>() { time };
            SpreadsheetsResource.ValuesResource.UpdateRequest updateTime = 
                service.Spreadsheets.Values.Update(vr, spreadsheetId, vr.Range);
            updateTime.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            updateTime.Execute();



            Console.WriteLine("Done");
        }
    }
}