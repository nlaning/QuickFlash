using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.Threading;
namespace QuickFlash
{
    public partial class MainWindow
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        //string sheetID = "11dac86fDnD7p-FutWQHP3l7voyMNDYLXhyIId0f44AU" ;
        string sheetID = "1_JajCnNTNvtMq5a0oiuTxIY0eVp82YfXY2EQUuuwCqg";//sheet id that contains the BIOS data
        string bios_task_and_products = "bios_tasks_and_products!A2:C";
        string bioses = "bioses!A2:K";
        IList<IList<Object>> bioses_data, bios_task_and_products_data;//making info global for ease of use
        IList<IList<Object>> ThumbdriveData;
        SheetsService service;
        UserCredential credentials;

        public void getSheetData()
        {
            getPermissions();
            createAPI();
            requestData();
            cleanData();
            
        }
        public void getPermissions()
        {
            
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credentailFile = "token.json";
                credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credentailFile, true)).Result;
                Console.WriteLine("Credential file saved to: " + credentailFile);

            }
        }
        public void createAPI()
        {
            // Create Google Sheets API service.
             service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "Google Sheets API .NET Quickstart",
            });
        }

        public void requestData()
        {
            SpreadsheetsResource.ValuesResource.GetRequest bioses_request =
                    service.Spreadsheets.Values.Get(sheetID, bioses);
            SpreadsheetsResource.ValuesResource.GetRequest bios_task_and_products_request =
                    service.Spreadsheets.Values.Get(sheetID, bios_task_and_products);
            ValueRange bioses_response = bioses_request.Execute();
            ValueRange bios_task_and_products_response = bios_task_and_products_request.Execute();
            bioses_data = bioses_response.Values;
            bios_task_and_products_data = bios_task_and_products_response.Values;
        }
        /**
         * removes Duplicated Data from BIOS sheet retrieved
         * */
        public void cleanData()
        {
            int finalValue = bioses_data.Count - 1;
            while (finalValue!=0)
            {
                
                string taskID = bioses_data[finalValue][2].ToString();
                for (int j= finalValue - 1; j >= 0; j--)
                {
                    if(bioses_data[j][2].ToString().Equals(taskID))
                    {
                        bioses_data.RemoveAt(j);
                        finalValue--;
                    }
                }
                finalValue--;
            }
        }

        
    }
}