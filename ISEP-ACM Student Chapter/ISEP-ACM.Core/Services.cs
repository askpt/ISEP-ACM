﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ISEP_ACM.Core
{
    public class Services
    {
        private const string _postsFileName = "Posts.json";
        private const string _videoFileName = "Videos.json";

        #region Posts
        private static async Task<string> GetAllPosts()
        {
            HttpClient client = new HttpClient();

            var baseUrl = "http://193.136.60.87/portal/?json=get_posts&count=20";

            string jsonPosts = await client.GetStringAsync(baseUrl);

            return jsonPosts;
        }

        public static Posts LoadPosts()
        {
            string jsonPosts;
            Posts posts = new Posts();

            using (IsolatedStorageFile storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storageFolder.FileExists(_postsFileName))
                {
                    return posts;
                }
                using (IsolatedStorageFileStream stream = storageFolder.OpenFile(_postsFileName, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonPosts = reader.ReadToEnd();
                        posts = JsonConvert.DeserializeObject<Posts>(jsonPosts);
                    }
                }
            }


            return posts;
        }

        public static async Task CreatePosts()
        {
            string stringData = await GetAllPosts();


            using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFolder.FileExists(_postsFileName))
                {
                    storageFolder.DeleteFile(_postsFileName);
                }

                using (var stream = storageFolder.CreateFile(_postsFileName))
                {
                    using (StreamWriter writter = new StreamWriter(stream))
                    {
                        writter.Write(stringData);
                    }
                }
            }
        }
        #endregion

        #region videos
        public static async Task<VideoSearch> GetVideosSearch()
        {
            HttpClient client = new HttpClient();

            var baseUri =
                "https://www.googleapis.com/youtube/v3/search?part=snippet&channelId=UCZdd-QeHpbPOSP-em5VW0zA&key=AIzaSyBjbuB4QjKjN73I2bAvOkaLGjIMIsE2x1U&maxResults=5";

            VideoSearch search = new VideoSearch();

            var str = await client.GetStringAsync(baseUri);

            search = JsonConvert.DeserializeObject<VideoSearch>(str);

            return search;
        }

        public static Video CreateVideoItem(Item item)
        {
            Video video = new Video();

            if (item.id.videoId != null)
            {
                video.VideoId = item.id.videoId;
                video.Title = item.snippet.title;
                video.Published = DateTime.Parse(item.snippet.publishedAt);
                video.Thumbnail = item.snippet.thumbnails.medium.url;
            }
            return video;
        }

        public static async Task CreateVideos()
        {
            VideoSearch search = await GetVideosSearch();
            RootVideo videos = new RootVideo();
            videos.Videos = new List<Video>();
            foreach (Item item in search.items)
            {
                if (item.id.videoId != null)
                {
                    Video videoToAdd = CreateVideoItem(item);
                    videos.Videos.Add(videoToAdd);
                }
            }

            string json = JsonConvert.SerializeObject(videos);

            using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFolder.FileExists(_videoFileName))
                {
                    storageFolder.DeleteFile(_videoFileName);
                }

                using (var stream = storageFolder.CreateFile(_videoFileName))
                {
                    using (StreamWriter writter = new StreamWriter(stream))
                    {
                        writter.Write(json);
                    }
                }
            }
        }

        public static List<Video> LoadVideos()
        {
            string json;
            var videos = new RootVideo()
            {
                Videos = new List<Video>()
            };

            using (IsolatedStorageFile storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storageFolder.FileExists(_postsFileName))
                {
                    return videos.Videos;
                }
                using (IsolatedStorageFileStream stream = storageFolder.OpenFile(_videoFileName, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        json = reader.ReadToEnd();
                        videos = JsonConvert.DeserializeObject<RootVideo>(json);
                    }
                }
            }

            return videos.Videos;
        }

        #endregion
    }
}
