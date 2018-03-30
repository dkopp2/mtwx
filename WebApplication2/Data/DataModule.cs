using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Autofac;

namespace Mtwx.Web.Data
{
    public enum DataConnectionType
    {
        AppReadWrite
    }

    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // register the Connections by Type
            builder.Register((c, p) =>
            {
                var type = p.TypedAs<DataConnectionType>();
                var connName = Enum.GetName(typeof(DataConnectionType), type);
                var connString = ConfigurationManager.ConnectionStrings[connName].ConnectionString;

                IDbConnection connection = new SqlConnection(connString);
                connection.Open();

                return connection;
            }).As<IDbConnection>();
        }
    }
}