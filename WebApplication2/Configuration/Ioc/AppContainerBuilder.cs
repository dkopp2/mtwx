using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace Mtwx.Web.Configuration.Ioc
{
    public class AppContainerBuilder
    {
        private readonly ContainerBuilder _builder;

        public AppContainerBuilder()
        {
            _builder = new ContainerBuilder();
        }

        public AppContainerBuilder WithAllModules()
        {
            return WithAllModules(null);
        }

        public AppContainerBuilder WithAllModules(IEnumerable<Type> noRegisterList)
        {
            // NOTE: This is mainly for testing. The caller can choose NOT to include a module or modules, if desired.
            noRegisterList = noRegisterList ?? Enumerable.Empty<Type>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var types = assemblies.SelectMany(x => x.GetTypes());
            var modules = types.Where(x => x.IsAssignableTo<IModule>() &&
                                            !x.IsAbstract &&
                                            !x.IsInterface &&
                                            noRegisterList.All(t => t != x)).ToList();

            modules
                .ForEach(x =>
                {
                    if (Activator.CreateInstance(x) is IModule m)
                    {
                        _builder.RegisterModule(m);
                    }
                });

            return this;
        }

        public AppContainerBuilder WithInstance<T>(T item)
            where T : class
        {
            _builder.RegisterInstance(item);
            return this;
        }

        public AppContainerBuilder WithModule<T>()
            where T : Module, new()
        {
            _builder.RegisterModule<T>();
            return this;
        }

        public AppContainerBuilder With<T>()
            where T : class
        {
            _builder.RegisterType<T>();
            return this;
        }

        public IContainer Build()
        {
            var retval = _builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(retval));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)retval); //Set the WebApi DependencyResolver
            return retval;
        }
    }
}