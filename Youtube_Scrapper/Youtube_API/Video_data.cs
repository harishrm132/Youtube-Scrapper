using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;

namespace Youtube_Scrapper
{
    class Video_data
    {

        static YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            ApiKey = Environment.GetEnvironmentVariable("API_KEY"),
            ApplicationName = "Test API"
        });

        //This will give us the full name path of the executable file:
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        //This will strip just the working path name:
        string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        DateTime now = DateTime.Now;

        public async Task Video_Metadata()
        {

            string InputFile = "C:\\Users\\haris\\Downloads\\Takeout\\YouTube and YouTube Music\\history\\watch-history.json";
            string lines = System.IO.File.ReadAllText(InputFile);
            List<Youtube> m = JsonConvert.DeserializeObject<List<Youtube>>(lines);

            List<string> IDlist = new List<string>();
            List<Metadata> dataarray = new List<Metadata>();


            //POSSIBLE ID VALUES
            foreach (Youtube s in m)
            {
                string ID = s.titleUrl;
                if (ID != null) { int found = ID.IndexOf("="); ID = ID.Substring(found + 1); IDlist.Add(ID); }
            }

            var videoListRequest = Video_data.youtubeService.Videos.List("snippet, contentDetails, statistics");

            for (int i = 0; i < IDlist.Count - 50; i += 50) // 
            {

                videoListRequest.Id = String.Join(',', IDlist.GetRange(i, 50));
                var Videolistreponse = await videoListRequest.ExecuteAsync();

                dataarray.AddRange(Videolistreponse.Items.Select(x => {
                    var data = new Metadata();
                    data.ID = x.Id;
                    data.Videotitle = x.Snippet.Title;
                    data.channelTitle = x.Snippet.ChannelTitle;
                    data.categoryID = x.Snippet.CategoryId;
                    data.VideoDate = x.Snippet.ChannelTitle;
                    data.VideoDuration = x.Snippet.ChannelTitle;

                    data.ViewCount = x.Statistics.ViewCount;
                    data.LikeCount = x.Statistics.LikeCount;
                    data.CommentsCount = x.Statistics.CommentCount;

                    return data;
                }));

                Console.WriteLine(i);

            }

            string OutputPath1 = strWorkPath + "/VideoDetails.json";
            SaveJSON<Metadata>(OutputPath1, dataarray);

        }

        public async Task Catogery_Data()
        {
            List<int> IDlist = new List<int>();
            List<Catogery> dataarray = new List<Catogery>();

            for (int i = 1; i <= 44; i++)
            {
                IDlist.Add(i);
            }

            var VideoCatogeryRequest = youtubeService.VideoCategories.List("snippet");
            VideoCatogeryRequest.Id = String.Join(',', IDlist.GetRange(0, 44));
            var VideoCatogeryReponse = await VideoCatogeryRequest.ExecuteAsync();

            dataarray.AddRange(VideoCatogeryReponse.Items.Select(x => {
                var data = new Catogery();
                data.ID = x.Id;
                data.title = x.Snippet.Title;

                return data;
            }));


            string OutputPath1 = strWorkPath + "/Catogery.json";
            SaveJSON <Catogery>(OutputPath1, dataarray);

        }

        private void SaveJSON <T> (String OutputPath, List<T> dataarray)
        {
            using (StreamWriter file = File.CreateText(OutputPath))
            {
                JsonSerializer JSsr = new JsonSerializer();
                //serialize object directly into file stream
                JSsr.Serialize(file, dataarray);
            }
        }

        public class Metadata
        {
            public string ID { get; set; } //ID
            public string Videotitle { get; set; } //Video Name
            public string channelTitle { get; set; } //ChannelName
            public string categoryID { get; set; } //ChannelCatogery
            public string VideoDate { get; set; } //Video published date
            public string VideoDuration { get; set; } //Video Duration

            public ulong? ViewCount { get; set; } //ViewCount
            public ulong? LikeCount { get; set; } //LikeCount
            public ulong? CommentsCount { get; set; } //CommentsCount
        }

        public class Catogery
        {
            public string ID { get; set; } //CatogeryID
            public string title { get; set; } //CatogeryTitle
        }

        public class Youtube
        {
            public string titleUrl { get; set; }
        }





    }
}
