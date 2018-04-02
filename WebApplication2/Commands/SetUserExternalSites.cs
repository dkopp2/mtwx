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
    public class SetUserExternalSites : IRequest<int>
    {
        public int ApplicationUserId { get; }
        public IEnumerable<int> ExternalSites { get; }

        public SetUserExternalSites(int applicationUserId, IEnumerable<int> externalSites)
        {
            ApplicationUserId = applicationUserId;
            ExternalSites = externalSites;
        }
    }

    public class SetUserExternalSitesHandler : SqlHandlerBase<SetUserExternalSites, int>
    {
        public SetUserExternalSitesHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(SetUserExternalSites message)
        {
            return string.Empty;
        }

        public override Task<int> Handle(SetUserExternalSites message, CancellationToken cancellationToken)
        {
            const string deleteCmd = "DELETE FROM ApplicationUserExternalSite WHERE ApplicationUserId = @ApplicationUserId; ";
            const string insertCmd = "INSERT INTO ApplicationUserExternalSite VALUES (@ApplicationUserId, @ExternalSiteId); ";

            return Task.Run(() =>
            {
                using (var cn = ConnectionFactory(ConnectionType))
                {
                    cn.Execute(deleteCmd, message);

                    // execute the insert command
                    var items = message.ExternalSites.Select(x =>
                        new { message.ApplicationUserId, ExternalSiteId = x.ToString() });
                    return cn.Execute(insertCmd, items);
                }
            }, cancellationToken);
        }
    }

}