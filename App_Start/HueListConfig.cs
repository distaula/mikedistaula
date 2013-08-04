using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Q42.HueApi;
using Q42.HueApi.Interfaces;

// ReSharper disable CheckNamespace
namespace BootstrapDiStaula
// ReSharper restore CheckNamespace
{
	public static class HueListConfig
	{
		public static void UpdateLights(IHueClient client, IList<Light> list)
		{
			Parallel.ForEach(list, light =>
			{
				Light initLight = client.GetLightAsync(light.Id).Result;
				initLight.Id = light.Id;
				list[list.IndexOf(light)] = initLight;
			});
		}

		public static void UpdateLight(IHueClient client, Light light)
		{
			light = client.GetLightAsync(light.Id).Result;
		}
	}
}
