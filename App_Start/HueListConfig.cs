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

		public static State DefaultState()
		{
			var state = new State();
			state.Alert = Alert.None;
			state.Brightness = 254;
			state.Hex = "FFB672";
			state.ColorMode = "hs";
			state.ColorTemperature = 369;
			state.Effect = Effect.None;
			state.Hue = 14922;
			state.On = true;
			state.Saturation = 144;

			return state;
		}
	}
}
