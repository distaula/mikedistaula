using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Q42.HueApi;
using BootstrapDiStaula.Models;
using BootstrapDiStaula.Extensions;
using Q42.HueApi.Interfaces;

namespace BootstrapDiStaula.Controllers
{
	public class LightsController : Controller
	{
		private readonly IHueClient _hueClient;
		private IList<Light> _lightList;

		public LightsController(IHueClient hueClient, IList<Light> lightList)
		{
			_hueClient = hueClient;
			_lightList = lightList;
		}

		public ViewResult Index()
		{
			return View(_lightList);
		}

		public ActionResult Refresh()
		{
			HueListConfig.UpdateLights(_hueClient, _lightList);

			return RedirectToAction("Index");
		}

		public ActionResult AllOff()
		{
			var command = new LightCommand();
			command = command.TurnOff();

			_hueClient.SendCommandAsync(command, _lightList);
			foreach (var light in _lightList)
			{
				light.State.On = false;
			}

			ViewBag.Msg = "Turned off Lights";

			return View("Index", _lightList);
		}

		public ActionResult AllOn()
		{
			var command = new LightCommand();
			command = command.TurnOn();

			_hueClient.SendCommandAsync(command, _lightList);
			foreach (var light in _lightList)
			{
				light.State.On = true;
			}

			ViewBag.Msg = "Turned on Lights";

			return View("Index", _lightList);
		}

		public ViewResult Details(string id)
		{
			Light light = _lightList.FirstOrDefault(x => x.Id == id);

			if (light == null)
				return View("Index", _lightList);

			return View(light);
		}

		// GET: /Default1/Edit/5
		public ActionResult Edit(string id)
		{
			Light light = _lightList.FirstOrDefault(x => x.Id == id);

			if (light == null)
				return RedirectToAction("Index");

			return View(light);
		}

		// POST: /Default1/Edit/5
		[HttpPost]
		public ActionResult Edit(Light light)
		{
			if (ModelState.IsValid)
			{
				Light lightIndex = _lightList.FirstOrDefault(x => x.Id == light.Id);
				lightIndex.State.On = light.State.On;
				lightIndex.State.Hex = light.State.Hex;
				var command = Mapper.Map<LightCommand>(lightIndex.State);

				_hueClient.SendCommandAsync(command, lightIndex);

				return RedirectToAction("Details");
			}
			else
			{
				return View();
			}
		}

		public ActionResult Toggle(string id)
		{
			Light light = _lightList.FirstOrDefault(x => x.Id == id);
			if (light != null)
			{
				light.State.On = !light.State.On;
				var command = Mapper.Map<LightCommand>(light.State);
				_hueClient.SendCommandAsync(command, light);
				ViewBag.Msg = string.Format("Toggled Light {0}", light.Name);
			}
			else
			{
				ViewBag.Msg = string.Format("Cannot find light {0}", id);
			}

			return View("Index", _lightList);
		}

		public ActionResult SetDefaults()
		{
			State state = HueListConfig.DefaultState();
			var command = Mapper.Map<LightCommand>(state);
			foreach (var light in _lightList)
			{
				light.State = state;
			}
			_hueClient.SendCommandAsync(command, _lightList);

			ViewBag.Msg = "Turned off Lights";

			return RedirectToAction("Index", _lightList);
		}
	}
}

