using FifteenGame.ClientProxy.Infrastructure;
using FifteenGame.Common.BusinessDtos;
using FifteenGame.Common.BusinessModels;
using FifteenGame.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using System.IO;

namespace FifteenGame.ClientProxy.Services
{
    public class UserServiceProxy : IUserService
    {
        public UserModel GetOrCreateUser(string userName)
        {


            //var httpContent = JsonContent.Create(new UserNameRequest { UserName = userName });
            var json = JsonConvert.SerializeObject(new UserNameRequest { UserName = userName });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = HttpConnection.HttpClient.PostAsync("api/users/create", httpContent).Result;
            response.EnsureSuccessStatusCode();

            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var reply = JsonConvert.DeserializeObject<UserReply>(jsonResponse);

            //var reply = JsonSerializer.Deserialize<UserReply>(response.Content.ReadAsStreamAsync().Result);
            return FromDto(reply);
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            var response = HttpConnection.HttpClient.GetAsync("api/users/get-all").Result;
            response.EnsureSuccessStatusCode();

            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var reply = JsonConvert.DeserializeObject<AllUsersReply>(jsonResponse);

            //var reply = JsonSerializer.Deserialize<AllUsersReply>(response.Content.ReadAsStreamAsync().Result);
            return reply.Users.Select(FromDto).ToList();
        }

        public UserModel GetUserByName(string userName)
        {
            var json = JsonConvert.SerializeObject(new UserNameRequest { UserName = userName });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            

            // var httpContent = JsonContent.Create(new UserNameRequest { UserName = userName });
            var response = HttpConnection.HttpClient.PostAsync("api/users/get-by-name", httpContent).Result;
            response.EnsureSuccessStatusCode();


            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var reply = JsonConvert.DeserializeObject<UserReply>(jsonResponse);


            //var reply = JsonSerializer.Deserialize<UserReply>(response.Content.ReadAsStreamAsync().Result);
            return FromDto(reply);
        }

        private UserModel FromDto(UserReply dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new UserModel
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }
    }
}
