using Autofac;
using Mtwx.Web.Commands;
using Mtwx.Web.Security;

namespace Mtwx.Web.Configuration.Ioc
{
    public class DefaultAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandFacade>().AsSelf().InstancePerLifetimeScope();
            builder.Register((c, p) =>
            {
                var cf = c.Resolve<CommandFacade>();

                return new DefaultLoginProvider(cf);
            }).As<ILoginProvider>();
        }
    }
}