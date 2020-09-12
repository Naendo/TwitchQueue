using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TwitchLib.PubSub.Models.Responses;

namespace TalkMaster.View.Controllers.HttpController
{
    public class CookieController
    {
        public void SetCookie(string key, string value, int? expireTimeInMinutes, HttpResponse response)
        {
            var options = new CookieOptions();

            if (expireTimeInMinutes.HasValue)
                options.Expires = DateTime.Now.AddMinutes(expireTimeInMinutes.Value);
            else
                options.Expires = DateTime.Now.AddMilliseconds(10);

            response.Cookies.Append(key, value, options);
        }

        public bool HasCookie(string key, HttpRequest request)
            => request.Cookies.ContainsKey(key);



        public string GetValue(string key, HttpRequest request)
            => request.Cookies[key];

        public void UpdateExpireTimeInMinutes(string key, string value, int expireTimeInMinutes, HttpResponse response)
        {
            var options = new CookieOptions();

            response.Cookies.Delete(key);

            SetCookie(key, value, expireTimeInMinutes, response);
        }

    }
}
