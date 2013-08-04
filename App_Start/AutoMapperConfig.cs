using AutoMapper;
using Q42.HueApi;

namespace BootstrapDiStaula.App_Start
{
	public class AutoMapperConfig
	{
		public static void Configure()
		{
			Mapper.CreateMap<State, LightCommand>();
		}
	}
}