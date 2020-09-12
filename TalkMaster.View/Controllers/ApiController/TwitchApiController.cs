using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using TalkMaster.View.Models;

namespace TalkMaster.View.Controllers.HttpController
{
    public class TwitchApiController : Controller
    {
        private readonly TokenOptions _tokens;

        public TwitchApiController(TokenOptions tokens)
        {
            _tokens = tokens;
        }

        public async Task<TokenModel> RequestToken(string responseCode)
        {
            var client = new RestClient("https://id.twitch.tv/oauth2");

            var request = new RestRequest($"token" +
                                          $"?client_id={_tokens.ClientId}" +
                                          $"&client_secret={_tokens.ClientSecret}" +
                                          $"&code={responseCode}" +
                                          $"&grant_type=authorization_code" +
                                          $"&redirect_uri={_tokens.RedirectUri}", Method.POST);


            var response = await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<TokenModel>(response.Content);
        }

        public async Task<UserModel> RequestUserInfo(TokenModel token)
        {
            var client = new RestClient($"https://api.twitch.tv/helix/users");

            var apiRequest = new RestRequest(Method.GET);

            apiRequest.AddHeader("Authorization", $"Bearer {token.AccessToken}");
            apiRequest.AddHeader("Client-ID", $"{_tokens.ClientId}");

            var result = await client.ExecuteAsync(apiRequest);
            return JsonConvert.DeserializeObject<UserModel>(result.Content);
        }
    }
}
