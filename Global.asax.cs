using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
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

			// Create the container builder.
			var builder = new ContainerBuilder();

			// Register the Web API controllers.
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

			// Register other dependencies.
			var client = new HueClient(HueConfig.HueIP);
			client.Initialize(HueConfig.HueMd5);
			builder.Register(c => client).As<IHueClient>().InstancePerApiRequest();


			// Build the container.
			var container = builder.Build();

			// Create the depenedency resolver.
			var resolver = new AutofacWebApiDependencyResolver(container);

			// Configure Web API with the dependency resolver.
			GlobalConfiguration.Configuration.DependencyResolver = resolver;
		}
	}

	internal static class HueConfig
	{
		public static string HueMd5 = ConfigurationManager.AppSettings["HueMd5"];
		public static string AppName = ConfigurationManager.AppSettings["AppName"];
		public static string HueIP = ConfigurationManager.AppSettings["HueIP"];
	}
}