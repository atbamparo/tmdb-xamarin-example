using Autofac;
using System.Net.Http;
using TMDbExample.Core.Repository;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Service;
using TMDbExample.Core.Service.API;

namespace TMDbExample.Forms.ViewModels
{
    public static class ViewModelLocator
    {
        private static IContainer _container;

        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<RequestHandler3>().As<IRequestHandler>()
                .WithParameter(new NamedParameter("apiKey", "API-KEY"));
            builder.RegisterType<HttpClient>().SingleInstance();

            builder.RegisterType<MoviesRepository>().As<IMoviesRepository>();
            builder.RegisterType<ConfigurationRepository>().As<IConfigurationRepository>();

            builder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            builder.RegisterType<MoviesService>().As<IMoviesService>();

            _container = builder.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
