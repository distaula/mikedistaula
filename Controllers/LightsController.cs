using System.Collections.Generic;
using System.Web.Mvc;
using Q42.HueApi.Interfaces;

namespace BootstrapDiStaula.Controllers
{
	public class LightsController : AsyncController
	{
		private readonly IEnumerable<string> _lightList = new List<string> { "1", "2", "3", "4", "6" };

		private IHueClient _hueClient;

		public LightsController(IHueClient hueClient)
		{
			_hueClient = hueClient;
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}
