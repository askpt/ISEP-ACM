using System.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net.Http;
using System.Threading.Tasks;

namespace ISEP_ACM.Core
{
    public static class Services
    {
        private const string PostsFileName = "Posts.json";
        private const string VideoFileName = "Videos.json";

        #region Posts
        private static async Task<string> GetAllPosts()
        {
            var client = new HttpClient();

            const string baseUrl = "http://193.136.60.87/portal/?json=get_posts&count=20";

            var jsonPosts = await client.GetStringAsync(baseUrl);

            return jsonPosts;
        }

        public static Posts LoadPosts()
        {
            var posts = new Posts();

            using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storageFolder.FileExists(PostsFileName))
                {
                    return posts;
                }
                using (var stream = storageFolder.OpenFile(PostsFileName, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var jsonPosts = reader.ReadToEnd();
                        posts = JsonConvert.DeserializeObject<Posts>(jsonPosts);
                    }
                }
            }


            return posts;
        }

        public static async Task CreatePosts()
        {
            var stringData = await GetAllPosts();

            using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFolder.FileExists(PostsFileName))
                {
                    storageFolder.DeleteFile(PostsFileName);
                }

                using (var stream = storageFolder.CreateFile(PostsFileName))
                {
                    using (var writter = new StreamWriter(stream))
                    {
                        writter.Write(stringData);
                    }
                }
            }
        }
        #endregion

        #region videos

        private static async Task<VideoSearch> GetVideosSearch()
        {
            var client = new HttpClient();

            const string baseUri = "https://www.googleapis.com/youtube/v3/search?part=snippet&channelId=UCZdd-QeHpbPOSP-em5VW0zA&key=AIzaSyBjbuB4QjKjN73I2bAvOkaLGjIMIsE2x1U&maxResults=5";

            var str = await client.GetStringAsync(baseUri);

            var search = JsonConvert.DeserializeObject<VideoSearch>(str);

            return search;
        }

        private static Video CreateVideoItem(Item item)
        {
            var video = new Video();

            if (item.id.videoId == null) 
                return video;

            video.VideoId = item.id.videoId;
            video.Title = item.snippet.title;
            video.Published = DateTime.Parse(item.snippet.publishedAt);
            video.Thumbnail = item.snippet.thumbnails.medium.url;

            return video;
        }

        public static async Task CreateVideos()
        {
            var search = await GetVideosSearch();
            var videos = new RootVideo
            {
                Videos = new List<Video>()
            };

            foreach (var videoToAdd in from item in search.items where item.id.videoId != null select CreateVideoItem(item))
            {
                videos.Videos.Add(videoToAdd);
            }

            var json = JsonConvert.SerializeObject(videos);

            using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFolder.FileExists(VideoFileName))
                {
                    storageFolder.DeleteFile(VideoFileName);
                }

                using (var stream = storageFolder.CreateFile(VideoFileName))
                {
                    using (var writter = new StreamWriter(stream))
                    {
                        writter.Write(json);
                    }
                }
            }
        }

        public static List<Video> LoadVideos()
        {
            var videos = new RootVideo
            {
                Videos = new List<Video>()
            };

            using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storageFolder.FileExists(PostsFileName))
                {
                    return videos.Videos;
                }
                using (var stream = storageFolder.OpenFile(VideoFileName, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var json = reader.ReadToEnd();
                        videos = JsonConvert.DeserializeObject<RootVideo>(json);
                    }
                }
            }

            return videos.Videos;
        }

        #endregion
    }
}
