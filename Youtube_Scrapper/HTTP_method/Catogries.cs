using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;



namespace Youtube_Scrapper
{
    class Catogries
    {

        static readonly HttpClient client = new HttpClient();
        public static List<Data1.Items> Conc = new List<Data1.Items>();

        public class Data1
        {
            public List<Items> items { get; set; }
            public class Items
            {
                public string ID { get; set; } //ID
                public Snippet snippet { get; set; }
                public class Snippet
                {
                    public string title { get; set; } //Catogery Name
                }
            } 
        }

        public async Task GetCatogries(String api_key)
        {

            //https://www.googleapis.com/youtube/v3/videoCategories?part=snippet&id=1%2C2%2C3&key=[YOUR_API_KEY]' \

            string link = "https://www.googleapis.com/youtube/v3/videoCategories";
            link = link + "?part=snippet&key=" + api_key;

            List<int> IDlist = new List<int>();
            for (int i = 1; i <= 44; i++)
            {
                IDlist.Add(i);
            }

            string Fulllink = link + "&id=" + String.Join(',', IDlist.GetRange(0, 44));
            string responseBody = await client.GetStringAsync(Fulllink);

            var m = JsonConvert.DeserializeObject<Data1>(responseBody);
            Conc.AddRange(m.items);


            //open file stream
            using (StreamWriter file = File.CreateText(@"C:\Catogery.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, Conc);
            }

            Console.WriteLine("Task Completed");
        }
    }
}
