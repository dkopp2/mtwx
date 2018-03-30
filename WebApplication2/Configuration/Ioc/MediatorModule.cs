using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac;
using MediatR;

namespace Mtwx.Web.Configuration.Ioc
{
    public class MediatorModule : Module
    {
        private const string AssemblyPrefixPattern = @"^Equipment\.Tracker";

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<>),
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            // register the handlers for all assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => Regex.IsMatch(x.FullName, AssemblyPrefixPattern) ||
                                                                                  x.FullName == GetType().Assembly.FullName);
            
            assemblies
                .Join(mediatrOpenTypes,
                            a => 1,
                            b => 1,
                            (a, b) => new { assembly = a, mediatrOpenType = b })
                .ToList()
                .ForEach(x =>
                {
                    builder
                        .RegisterAssemblyTypes(x.assembly)
                        .AsClosedTypesOf(x.mediatrOpenType)
                        .AsImplementedInterfaces();
                });

            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();

                return t =>
                {
                    var inst = c.TryResolve(t, out var o) ? o : null;
                    return inst;
                };
            });

            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

            // these should be registered from general to specific
            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
 
        }
    }
}