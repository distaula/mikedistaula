using System.Web.Optimization;

// ReSharper disable CheckNamespace
namespace BootstrapDiStaula
// ReSharper restore CheckNamespace
{
	public static class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			RegisterStyleBundles(bundles);
			RegisterJavascriptBundles(bundles);
			RegisterAnalyticsBundle(bundles);
		}

		private static void RegisterAnalyticsBundle(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/analytics")
							.Include("~/Scripts/Analytics/analytics.js"));
		}

		private static void RegisterStyleBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/css")
							.Include("~/Content/bootstrap.css")
							.Include("~/Content/bootstrap-responsive.css")
							.Include("~/Content/site.css")
							.Include("~/Content/jquery.minicolors.css"));
		}

		private static void RegisterJavascriptBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/js")
							.Include("~/Scripts/jquery-{version}.js")
							.Include("~/Scripts/jquery-ui-{version}.js")
							.Include("~/Scripts/jquery-ui.unobtrusive-{version}.js")
							.Include("~/Scripts/bootstrap.js")
							.Include("~/Scripts/jquery.minicolors.js")
							.Include("~/Scripts/protofm.js")
							.Include("~/Scripts/protovis.js")
							.Include("~/Scripts/protovis.min.js")
							.Include("~/Scripts/d3.js")
							.Include("~/Scripts/d3.chart.js")
							.Include("~/Scripts/d3.layout.js"));
		}
	}
}