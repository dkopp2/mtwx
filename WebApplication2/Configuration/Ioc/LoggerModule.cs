using Autofac;
using Serilog;

namespace Mtwx.Web.Configuration.Ioc
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) => new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger()).AsSelf().As<ILogger>().InstancePerLifetimeScope();
        }
    }
}