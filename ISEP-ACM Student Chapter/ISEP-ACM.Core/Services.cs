using Newtonsoft.Json;
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
        private const string _fileName = "Posts.json";

        private static async Task<string> GetAllPosts()
        {
            HttpClient client = new HttpClient();

            var baseUrl = "http://193.136.60.87/portal/?json=get_posts";

            string jsonPosts = await client.GetStringAsync(baseUrl);

            return jsonPosts;
        }

        public static async Task<Posts> LoadPosts()
        {
            string jsonPosts;

            using (IsolatedStorageFile storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storageFolder.FileExists(_fileName))
                {
                    await CreatePosts();
                }


                using (IsolatedStorageFileStream stream = storageFolder.OpenFile(_fileName, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonPosts = reader.ReadToEnd();
                    }
                }
            }

            Posts posts = JsonConvert.DeserializeObject<Posts>(jsonPosts);

            return posts;
        }

        public static async Task CreatePosts()
        {
            string stringData = await GetAllPosts();


            using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFolder.FileExists(_fileName))
                {
                    storageFolder.DeleteFile(_fileName);
                }

                using (var stream = storageFolder.CreateFile(_fileName))
                {
                    using (StreamWriter writter = new StreamWriter(stream))
                    {
                        writter.Write(stringData);
                    }
                }
            }
        }

    }
}
