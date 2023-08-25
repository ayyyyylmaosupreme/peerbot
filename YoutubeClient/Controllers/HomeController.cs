using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YoutubeClient.Models;
using ChatBot.MessageSpeaker;
using System.IO;
using ChatBot;
using TwitchLib.Client.Events;
using YoutubeClient.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace YoutubeClient.Controllers
{
    public class HomeController : Controller
    {

        private string links = null;

        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ChatHub> chatHub = null;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.googleTTSVoices = Startup.GoogleTTSVoices.Select(item => new SelectListItem { Value = item.id.ToString(), Text = item.GetDisplayName() }).ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetTTSVoice([FromBody] GoogleTTSVoice voice)
        {
            GoogleTTSVoice googleVoice = Startup.GoogleTTSVoices[voice.id];

            UserTTSSettings settings = new UserTTSSettings();
            settings.twitchUsername = voice.username;
            settings.ttsSettings.SetSpeed(voice.speed);
            settings.ttsSettings.SetPitch(voice.pitch);
            settings.ttsSettings.SetGender(googleVoice.gender);
            settings.ttsSettings.languageCode = googleVoice.languageCode;
            settings.ttsSettings.voiceName = googleVoice.languageName;

            UserTTSSettingsManager.SaveSettingsToStorage(settings);
            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
