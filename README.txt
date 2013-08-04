AutoMapper4Mvc

AutoMapper4Mvc is a small project that contains a couple of useful features for working with AutoMapper in ASP.Net MVC and Web API
projects.  

Attributes

AutoMapper4Mvc contains attributes that can be used to map a model returned by an MVC or Web API action to another type.  This
capability allows you to keep dependencies on AutoMapper and mapping code out of your controller.

Profile Loader

AutoMapper4Mvc contains a status method, LoadProfiles that will scan all assemblies loaded by your web application and will configure
AutoMapper with any AutoMapper profiles found in those assemblies.

Examples

MVC Attribute

    public class HomeController : Controller
    {
      private readonly IProductService _productService;
      
      [AutoMap(typeof(ProductCollection), typeof(ProductListViewModel))]
      public ActionResult Index()
      {
	    return View(_productService.GetProductList());
      }
    }

In the above example, the view for this controller action would receive a model
of type ProductListViewModel

Web API Attribute

    public class ProductsController : ApiController
    {
      private readonly IProductService _productService;
      
      [AutoMap(typeof(ProductCollection), typeof(List<Product>))]
      public ProductCollection Get()
      {
        return _productService.GetProductList();
      }
    }

In this example, a client that calls /products would receive a List<Product> response.

Profile Loader

    public class MvcApplication : System.Web.HttpApplication
	{
	  protected void Application_Start()
	  {
	    // Other code ommited
		ThirteenDaysAWeek.AutoMapper4Mvc.Configuration.ProfileLoader.LoadProfiles();
	  }
	}

In this example, all AutoMapper profiles contained in the web application and any assemblies it
references will be loaded at application start.