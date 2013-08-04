using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Q42.HueApi;
using BootstrapDiStaula.Models;
using Q42.HueApi.Interfaces;

namespace BootstrapDiStaula.Controllers
{
	public class LightsController : AsyncController
	{
		private readonly IHueClient _hueClient;
		private IEnumerable<string> _lightList;

		public LightsController(IHueClient hueClient, IEnumerable<string> lightList)
		{
			_hueClient = hueClient;
			_lightList = lightList;
		}

		//
		// GET: /Default1/

		public async Task<ViewResult> Index()
		{
			var lights = await _hueClient.GetLightsAsync();
			var lightList = lights.Where(x => _lightList.Contains(x.Id));

			return View(lightList);
		}

		//
		// GET: /Default1/Details/5

		public async Task<ViewResult> Details(string id)
		{
			return View(await _hueClient.GetLightAsync(id));
		}

		//
		// GET: /Default1/Create

		public async Task<ActionResult> Create()
		{
			return View();
		}

		//
		// POST: /Default1/Create

		[HttpPost]
		public async Task<ActionResult> Create(Light light)
		{
			if (ModelState.IsValid)
			{
				//lightRepository.InsertOrUpdate(light);
				//lightRepository.Save();
				return RedirectToAction("Index");
			}
			else
			{
				return View();
			}
		}

		//
		// GET: /Default1/Edit/5

		public async Task<ActionResult> Edit(string id)
		{
			return View(await _hueClient.GetLightAsync(id));
		}

		//
		// POST: /Default1/Edit/5

		[HttpPost]
		public async Task<ActionResult> Edit(Light light)
		{
			if (ModelState.IsValid)
			{
				//lightRepository.InsertOrUpdate(light);
				//lightRepository.Save();
				return RedirectToAction("Index");
			}
			else
			{
				return View();
			}
		}

		//
		// GET: /Default1/Delete/5

		public async Task<ActionResult> Delete(string id)
		{
			return View(await _hueClient.GetLightAsync(id));
		}

		//
		// POST: /Default1/Delete/5

		[HttpPost, ActionName("Delete")]
		public async Task<ActionResult> DeleteConfirmed(string id)
		{
			//lightRepository.Delete(id);
			//lightRepository.Save();

			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				//lightRepository.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}

