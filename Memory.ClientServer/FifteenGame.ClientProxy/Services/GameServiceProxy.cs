using FifteenGame.ClientProxy.Infrastructure;
using FifteenGame.Common.BusinessDtos;
using FifteenGame.Common.BusinessModels;
using FifteenGame.Common.Definitions;
using FifteenGame.Common.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FifteenGame.ClientProxy.Services
{
    public class GameServiceProxy : IGameService
    {
        

        public GameModel GetByGameId(int gameId)
        {

            var json = JsonConvert.SerializeObject(new GetByGameIdRequest { GameId = gameId });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //var httpContent = JsonContent.Create(new GetByGameIdRequest { GameId = gameId });
            var response = HttpConnection.HttpClient.PostAsync("api/games/get-by-id", httpContent).Result;
            response.EnsureSuccessStatusCode();

            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var reply = JsonConvert.DeserializeObject<GameReply>(jsonResponse);

            //var reply = JsonSerializer.Deserialize<GameReply>(response.Content.ReadAsStreamAsync().Result);
            return FromDto(reply);
        }

        public GameModel GetByUserId(int userId)
        {

            var json = JsonConvert.SerializeObject(new GetByUserIdRequest { UserId = userId });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //var httpContent = JsonContent.Create(new GetByUserIdRequest { UserId = userId });
            var response = HttpConnection.HttpClient.PostAsync("api/games/get-by-user-id", httpContent).Result;
            response.EnsureSuccessStatusCode();

            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var reply = JsonConvert.DeserializeObject<GameReply>(jsonResponse);

            //var reply = JsonSerializer.Deserialize<GameReply>(response.Content.ReadAsStreamAsync().Result);
            return FromDto(reply);
        }

        public bool IsGameOver(int gameId)
        {
            var json = JsonConvert.SerializeObject(new GetByGameIdRequest { GameId = gameId });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //var httpContent = JsonContent.Create(new GetByGameIdRequest { GameId = gameId });
            var response = HttpConnection.HttpClient.PostAsync("api/games/is-game-over", httpContent).Result;
            response.EnsureSuccessStatusCode();

            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var reply = JsonConvert.DeserializeObject<BooleanReply>(jsonResponse);

            //var reply = JsonSerializer.Deserialize<BooleanReply>(response.Content.ReadAsStreamAsync().Result);
            return reply.Value;
        }

        public GameModel CheckMatch(int gameId, int[] OneColumnRow, int[] TwoColumnRow)
        {
            var json = JsonConvert.SerializeObject(new MakeMoveRequest { GameId = gameId, OneColumnRow = OneColumnRow, TwoColumnRow = TwoColumnRow });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //var httpContent = JsonContent.Create(new MakeMoveRequest { GameId = gameId, OneColumnRow = OneColumnRow, TwoColumnRow = TwoColumnRow });
            var response = HttpConnection.HttpClient.PostAsync("api/games/make-move", httpContent).Result;
            response.EnsureSuccessStatusCode();


            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var reply = JsonConvert.DeserializeObject<GameReply>(jsonResponse);

            //var reply = JsonSerializer.Deserialize<GameReply>(response.Content.ReadAsStreamAsync().Result);
            return FromDto(reply);
        }

        public void RemoveGame(int gameId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/games/remove");

            var json = JsonConvert.SerializeObject(new GetByGameIdRequest { GameId = gameId });


            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            //request.Content = JsonContent.Create(new GetByGameIdRequest { GameId = gameId });

            var response = HttpConnection.HttpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
        }

        private GameModel FromDto(GameReply game)
        {
            var model = new GameModel
            {
                GameId = game.GameId,
                UserId = game.UserId,
                GameStart = game.GameStart,
                MoveCount = game.MoveCount,
                
            };

            int i = 0;
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    model[row, column] = game.Cells[i++];
                }
            }

            return model;
        }
    }
}
