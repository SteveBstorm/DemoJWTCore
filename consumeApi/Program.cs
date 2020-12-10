using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace consumeApi
{
    class Program
    {
        static async Task<List<User>> Get(string token)
        {
            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:50894/api/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            HttpResponseMessage message = await _client.GetAsync("User/list");
            string json = message.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<User>>(json);
        }

        

        static async Task<User> Post(UserLoginInfo f)
        {
            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:50894/api/");
            string json = JsonConvert.SerializeObject(f);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await _client.PostAsync("User/auth", content))
            {
                if (!response.IsSuccessStatusCode) { throw new HttpRequestException(); }
                string resp = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<User>(resp);
            }
            
        }

        static void Main(string[] args)
        {

            UserLoginInfo loginInfo = new UserLoginInfo
            {
                UserName = "terminator",
                Password = "test"
            };

            User user = Post(loginInfo).Result;

            Console.WriteLine(user.FirstName + ' ' +user.LastName);
            Console.WriteLine(user.Token);


            foreach (User u in Get(user.Token).Result)
            {
                Console.WriteLine(u.LastName);
            }


            Console.WriteLine("Hello World!");
        }
    }
}
