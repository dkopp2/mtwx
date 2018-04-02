using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class SetRoleExternalSites : IRequest<int>
    {
        public int ApplicationRoleId { get; }
        public IEnumerable<int> ExternalSites { get; }

        public SetRoleExternalSites(int applicationRoleId, IEnumerable<int> externalSites)
        {
            ApplicationRoleId = applicationRoleId;
            ExternalSites = externalSites;
        }
    }

    public class SetRoleExternalSitesHandler : SqlHandlerBase<SetRoleExternalSites, int>
    {
        protected override string GetSql(SetRoleExternalSites message)
        {
            return string.Empty;
        }

        public override Task<int> Handle(SetRoleExternalSites message, CancellationToken cancellationToken)
        {
            const string deleteCmd = "DELETE FROM ApplicationRoleExternalSite WHERE ApplicationRoleId = @ApplicationRoleId; ";
            const string insertCmd = "INSERT INTO ApplicationRoleExternalSite VALUES (@ApplicationRoleId, @ExternalSiteId); ";

            return Task.Run(() =>
            {
                using (var cn = ConnectionFactory(ConnectionType))
                {
                    cn.Execute(deleteCmd, message);

                    // execute the insert command
                    var items = message.ExternalSites.Select(x =>
                        new { message.ApplicationRoleId, ExternalSiteId = x.ToString() });
                    return cn.Execute(insertCmd, items);
                }
            }, cancellationToken);
        }

        public SetRoleExternalSitesHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }
    }
}