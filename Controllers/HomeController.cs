using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Web.UI;
using BootstrapDiStaula.Models;
using Q42.HueApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Q42.HueApi.Interfaces;
using Q42.HueApi.NET;

namespace BootstrapDiStaula
{
	public class HomeController : AsyncController
	{
		private readonly IEnumerable<string> _lightList = new List<string> { "1", "2", "3", "4", "6" };

		private IHueClient _hueClient;
		public HomeController(IHueClient hueClient)
		{
			_hueClient = hueClient;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Examples()
		{
			return View();
		}

		public ActionResult HueAdmin()
		{
			return View(new AdminViewModel());
		}

		[HttpPost]
		public ActionResult HueAdmin(AdminViewModel model)
		{
			var command = new LightCommand();
			command = command.SetColor(model.Color.Remove(0, 1));
			command.Effect = Effect.None;

			_hueClient.SendCommandAsync(command, _lightList);

			return View("HueAdmin", new AdminViewModel { Log = "Changed Color" });
		}

		public ActionResult TurnOff()
		{
			var command = new LightCommand();
			command = command.TurnOff();
			command.Effect = Effect.None;

			_hueClient.SendCommandAsync(command, _lightList);

			return View("HueAdmin", new AdminViewModel { Log = "Turned Off" });
		}

		public async Task<ActionResult> Toggle()
		{
			var light = await _hueClient.GetLightAsync(_lightList.FirstOrDefault());

			if (light.State.On)
				return TurnOff();

			return TurnOn();
		}

		public ActionResult TurnOn()
		{
			var command = new LightCommand();
			command = command.TurnOn();
			command.Effect = Effect.None;

			_hueClient.SendCommandAsync(command, _lightList);

			return View("HueAdmin", new AdminViewModel { Log = "Turned On" });
		}

		public ActionResult SetWheel()
		{
			var command = new LightCommand();
			command = command.TurnOn();
			command.Effect = Effect.ColorLoop;

			_hueClient.SendCommandAsync(command, new List<string> { "5" });

			return View("HueAdmin", new AdminViewModel { Log = "SetWheel" });
		}

		public ActionResult SetColor(string color)
		{
			throw new NotImplementedException();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Location()
		{
			return View();
		}

		public ActionResult Music()
		{
			return View();
		}

		//[OutputCache(Duration = 600, VaryByParam = "none", Location = OutputCacheLocation.Server, NoStore = true)]
		public async Task<ActionResult> MusicChart()
		{
			int _maxItems = 15; // default max tracks to display
			int _refreshInterval = 0; // Number of minutes to reload source data - 0 refreshes every load
			string _imageSize = "small"; // default image size
			string _userName = ConfigurationManager.AppSettings["LastFMUserName"];
			string _apiKey = ConfigurationManager.AppSettings["LastFMApiKey"];
			string _secret = ConfigurationManager.AppSettings["LastFMSecret"];
			int _limit = 11;
			var barHeight = 34;
			var barSpacing = barHeight + 1;
			var chartWidth = 624;
			var chartHeight = barSpacing * _limit;

			//string _url =
			//	string.Format(
			//		@"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user={0}&period=7day&api_key={1}&limit={2}",
			//		_userName, _apiKey, _limit);
			//string _json = "&format=json";

			//var request = WebRequest.Create(_url + _json) as HttpWebRequest;
			//HttpClient client = new HttpClient();

			//request.Method = "POST";
			//var response = await client.PostAsync(_url + _json);

			//using (TextReader reader = new StreamReader(response.GetResponseStream()))
			//{
			//	str = reader.ReadToEnd();
			//}

			List<Artist> test = await TopArtists.GetWeeklyTopArtists();

			JsonResult json = new JsonResult();
			json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			json.Data = test;

			return json;
		}
	}

	public class AdminViewModel
	{
		public string Log { get; set; }
		public bool Result { get; set; }
		public string Color { get; set; }
		public bool Mode { get; set; }
	}
}
