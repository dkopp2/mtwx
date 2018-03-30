using System;
using System.Linq;
using System.Reflection;
using Autofac;

namespace Mtwx.Web.Configuration.Ioc
{
    public static class ContainerFactory
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<IContainer> _defaultContainer = new Lazy<IContainer>(BuildContainer);

        private static IContainer BuildContainer()
        {
            try
            {
                return new AppContainerBuilder().WithAllModules().Build();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                throw new LoadingApplicationTypesException(rtle.LoaderExceptions.FirstOrDefault());
            }
        }



        public static IContainer DefaultContainer => _defaultContainer.Value;

    }
}