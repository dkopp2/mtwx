using System;

namespace Mtwx.Web.Configuration.Ioc
{
    public class LoadingApplicationTypesException : Exception
    {
        public LoadingApplicationTypesException(Exception inner) : base("Unable to load types.",
            inner)
        {
        }
    }
}