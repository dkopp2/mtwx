using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace Mtwx.Web.Configuration.Ioc
{
    public class MvcModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var asm = typeof(MvcApplication).Assembly;
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(asm);
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();

            builder.RegisterControllers(asm);
            builder.RegisterModelBinders(asm);
            builder.RegisterModelBinderProvider();
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterSource(new ViewRegistrationSource());
        }
    }
}