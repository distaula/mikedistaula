﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Q42.HueApi;
using Q42.HueApi.Interfaces;

namespace BootstrapDiStaula.Controllers
{
	public class LightsController : ApiController
	{
		private readonly IEnumerable<string> _lightList = new List<string> { "1", "2", "3", "4", "6" };

		private IHueClient _hueClient;
		public LightsController(IHueClient hueClient)
		{
			_hueClient = hueClient;
		}

		// GET api/hue
		public async Task<IEnumerable<Light>> GetAllLights()
		{
			var lights = await _hueClient.GetLightsAsync();
			var lightList = lights.Where(x => _lightList.Contains(x.Id));
			return lightList;
		}

		// GET api/hue/5
		public async Task<Light> Get(int id)
		{
			var light = await _hueClient.GetLightAsync("" + id);
			return light;
		}

		// POST api/hue
		public void Post([FromBody]Light value)
		{
		}

		// PUT api/hue/5
		public void Put(int id, [FromBody]Light value)
		{
		}

		// DELETE api/hue/5
		//public void Delete(int id)
		//{
		//}
	}
}
