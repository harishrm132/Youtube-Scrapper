using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace Youtube_Scrapper
{
    class Program
    {

        static async Task Main(string[] args)
        {
            //Call Google API
            Video_data CT = new Video_data();
            await CT.Catogery_Data();
            await CT.Video_Metadata();

            //Call HTTP Method
            //Catogries CT = new Catogries();
            //await CT.GetCatogries(api_key);
            //VideoDetails VD = new VideoDetails();
            //await VD.GetVideo(api_key);
        }
    }


 
}
