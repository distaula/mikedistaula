using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Q42.HueApi;
using Q42.HueApi.Interfaces;

namespace BootstrapDiStaula.Extensions
{
	public static class HueClientExtensions
	{
		public static Task SendCommandAsync(this IHueClient hueClient, LightCommand command, IEnumerable<Light> lights)
		{
			return hueClient.SendCommandAsync(command, lights.Select(x => x.Id));
		}

		public static Task SendCommandAsync(this IHueClient hueClient, LightCommand command, Light light)
		{
			return hueClient.SendCommandAsync(command, new[] { light.Id });
		}

		public static Task SendCommandAsync(this IHueClient hueClient, LightCommand command, string lightId)
		{
			return hueClient.SendCommandAsync(command, new[] { lightId });
		}

		public static void UpdateLights(this IHueClient hueClient, IEnumerable<Light> lights)
		{
			IEnumerable<Light> lights1 = lights;
			lights = hueClient.GetLightsAsync().Result.Where(x => lights1.Select(l => l.Id).Contains(x.Id));
		}

		public static void UpdateLight(this IHueClient hueClient, Light light)
		{
			light = hueClient.GetLightAsync(light.Id).Result;
		}
	}
}