using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ISEP_ACM.Core
{
    public class Services
    {
        public static async Task<Posts> GetAllPosts()
        {
            HttpClient client = new HttpClient();

            var baseUrl = "http://193.136.60.87/portal/?json=get_posts";

            string jsonPosts = await client.GetStringAsync(baseUrl);

            Posts posts = JsonConvert.DeserializeObject<Posts>(jsonPosts);      

            return posts;
        
        }

    }
}
