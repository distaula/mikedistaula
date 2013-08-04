using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Q42.HueApi;
using BootstrapDiStaula.Models;

namespace BootstrapDiStaula.Controllers
{
	public class LightsController : Controller
	{
		private readonly ILightRepository lightRepository;

		// If you are using Dependency Injection, you can delete the following constructor
		public LightsController()
			: this(new LightRepository())
		{
		}

		public LightsController(ILightRepository lightRepository)
		{
			this.lightRepository = lightRepository;
		}

		//
		// GET: /Default1/

		public ViewResult Index()
		{
			return View(lightRepository.All);
		}

		//
		// GET: /Default1/Details/5

		public ViewResult Details(string id)
		{
			return View(lightRepository.Find(id));
		}

		//
		// GET: /Default1/Create

		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Default1/Create

		[HttpPost]
		public ActionResult Create(Light light)
		{
			if (ModelState.IsValid)
			{
				lightRepository.InsertOrUpdate(light);
				lightRepository.Save();
				return RedirectToAction("Index");
			}
			else
			{
				return View();
			}
		}

		//
		// GET: /Default1/Edit/5

		public ActionResult Edit(string id)
		{
			return View(lightRepository.Find(id));
		}

		//
		// POST: /Default1/Edit/5

		[HttpPost]
		public ActionResult Edit(Light light)
		{
			if (ModelState.IsValid)
			{
				lightRepository.InsertOrUpdate(light);
				lightRepository.Save();
				return RedirectToAction("Index");
			}
			else
			{
				return View();
			}
		}

		//
		// GET: /Default1/Delete/5

		public ActionResult Delete(string id)
		{
			return View(lightRepository.Find(id));
		}

		//
		// POST: /Default1/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(string id)
		{
			lightRepository.Delete(id);
			lightRepository.Save();

			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				lightRepository.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}

