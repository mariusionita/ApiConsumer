using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.IO;
using System;
using System.Text;

namespace ApiConsumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (WebClient wc = new WebClient()) 
            {
                string jsonInputString = wc.DownloadString("https://graph.cloud.selfpay.ro/v2/opendata/terminals/locations");
                List<JsonItem> outputList = JsonConvert.DeserializeObject<List<JsonItem>>(jsonInputString);
                List<string> citiesList = new List<string>();
                
                foreach (var item in outputList)
                {
                    if (!citiesList.Contains(item.City))
                        citiesList.Add(item.City);
                }

                foreach (var item in citiesList) 
                {
                    List<JsonItem> result = outputList.FindAll(c => c.City == item);
                    List<string> listAfterConversion = new List<string>();
                    StringBuilder headerLine = new StringBuilder(string.Empty);
                    if(result.Count>0)
                        headerLine.Append(nameof(JsonItem.Name) + "," + nameof(JsonItem.City) + "," + nameof(JsonItem.Address) + "," + nameof(JsonItem.Latitude) + "," + nameof(JsonItem.Longitude) + "," + nameof(JsonItem.Country) + "," + nameof(JsonItem.LocationId));
                    listAfterConversion.Add(headerLine.ToString());

                    foreach (var itemList in result)
                        listAfterConversion.Add(itemList.Name + "," + itemList.City+ "," + itemList.Address + "," + itemList.Latitude + "," + itemList.Longitude + "," + itemList.Country + "," + itemList.LocationId);
                    File.WriteAllLines(item + ".csv", listAfterConversion.ToArray());
                }

                Console.WriteLine("The work is DONE.");
                Console.ReadKey();
            }
        }
    }
}
