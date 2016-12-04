using Microsoft.Extensions.DependencyInjection;
using PairingTest.Application;
using PairingTest.Application.Abstractions;
using PairingTest.Web.App_Start;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PairingTest.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ConfigurateDependencies();
        }

        public void ConfigurateDependencies()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());
            DependencyResolver.SetResolver(resolver);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Controllers.QuestionnaireController>();
            services.AddTransient<IQuestionServiceConsumer, QuestionServiceConsumer>();
            services.AddScoped<IHttpClientHelper, HttpClientHelper>();
            services.AddScoped<IApiBaseUrlProvider, ApiBaseUrlProvider>();
        }
    }
}