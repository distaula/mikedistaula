using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Q42.HueApi;
using Q42.HueApi.Interfaces;

namespace BootstrapDiStaula.Controllers.Api
{
	public class LightsController : ApiController
	{
		private readonly IEnumerable<string> _lightList;

		private IHueClient _hueClient;
		public LightsController(IHueClient hueClient, IEnumerable<string> lightList)
		{
			_hueClient = hueClient;
			_lightList = lightList;
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
			var command = Mapper.Map<LightCommand>(value.State);
			var command2 = command;


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

	public static class LightExtensions
	{
		public static LightCommand GetCommand(this Light light)
		{
			var command = new LightCommand();
			command.Alert = light.State.Alert;
			command.Brightness = light.State.Brightness;

			return null;
		}
	}
}
