using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Collections.Generic;
using AutoMapper;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using BootstrapDiStaula.App_Start;
using Glimpse.Core.Extensibility;
using Q42.HueApi;
using Q42.HueApi.Interfaces;

namespace BootstrapDiStaula
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			AutoMapperConfig.Configure();


			// Create the container builder.
			var builder = new ContainerBuilder();

			// Register the Web API controllers.
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			builder.RegisterControllers(Assembly.GetExecutingAssembly());

			// Register other dependencies.
			var client = new HueClient(HueConfig.HueIP);
			client.Initialize(HueConfig.HueMd5);
			builder.Register(c => client).As<IHueClient>().SingleInstance();

			var list = new List<Light>
				{
					new Light {Id = "1"},
					new Light {Id = "2"},
					new Light {Id = "3"},
					new Light {Id = "4"},
					new Light {Id = "6"}
				};

			HueListConfig.UpdateLights(client, list);

			builder.Register(l => list).As<IList<Light>>().SingleInstance();

			// Build the container.
			var container = builder.Build();

			// Create the depenedency resolver.
			var webApiResolver = new AutofacWebApiDependencyResolver(container);

			// Configure Web API with the dependency resolver.
			GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

		}
	}

	//public async Task<ActionResult> Register()
	//{
	//	var viewModel = new AdminViewModel();
	//	var locator = new SSDPBridgeLocator();

	//	//For Windows 8 and .NET45 projects you can use the SSDPBridgeLocator which actually scans your network. 
	//	//See the included BridgeDiscoveryTests and the specific .NET and .WinRT projects
	//	IEnumerable<string> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(15));

	//	HueClient client = new HueClient(HueConfig.HueIP);
	//	viewModel.Result = await client.RegisterAsync(HueConfig.AppName, HueConfig.HueMd5);

	//	if (viewModel.Result)
	//	{
	//		viewModel.Log = "Successfully registered application";
	//		return View("HueAdmin", viewModel);
	//	}

	//	viewModel.Log = "Failed to register application";
	//	return View("HueAdmin", viewModel);
	//}

	internal static class HueConfig
	{
		public static string HueMd5 = ConfigurationManager.AppSettings["HueMd5"];
		public static string AppName = ConfigurationManager.AppSettings["AppName"];
		public static string HueIP = ConfigurationManager.AppSettings["HueIP"];
	}
}