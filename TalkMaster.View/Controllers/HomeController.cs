using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using TalkMaster.Services.Services;
using TalkMaster.View.Controllers.HttpController;
using TalkMaster.View.Models;
using TalkMaster.View.Models.Options;

namespace TalkMaster.View.Controllers
{
    public class HomeController : Controller
    {

        private readonly TwitchApiController _twitchApiController;
        private readonly CookieController _cookieController;
        private readonly TokenOptions _tokenOptions;
        private readonly PaypalOptions _paypalOptions;




        public HomeController(TwitchApiController twitchApiController, CookieController cookieController, TokenOptions tokenOptions, PaypalOptions paypalOptions, IWebHostEnvironment env)
        {
            _twitchApiController = twitchApiController;
            _cookieController = cookieController;
            _tokenOptions = tokenOptions;
            _paypalOptions = paypalOptions;
            
        }

        public async Task<IActionResult> Index(string code)
        {
            _tokenOptions.ForceVerify = false;

            if (code == null) return RedirectToAction("Login");

            if (_cookieController.HasCookie("user", Request))
            {
                var userValue = JsonConvert.DeserializeObject<UserModel>(_cookieController.GetValue("user", Request));

                //2888 = 2ds
                _cookieController.UpdateExpireTimeInMinutes("user", _cookieController.GetValue("user", Request), 2888,
                    Response);

                ViewData["user"] = userValue.Data.FirstOrDefault();
                return View();
            }

            var token = await _twitchApiController.RequestToken(code);

            var user = await _twitchApiController.RequestUserInfo(token);

            _cookieController.SetCookie("user", JsonConvert.SerializeObject(user), 2888, Response);

            ViewData["user"] = user.Data.FirstOrDefault();

            return View();

        }

        public IActionResult Login()
        {
            if (_cookieController.HasCookie("user", Request))
                return RedirectToAction("Index", routeValues: new { code = "#" });

            _tokenOptions.ForceVerify = true;
            ViewData["token"] = _tokenOptions;
            return View();
        }


        public IActionResult Strike()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
