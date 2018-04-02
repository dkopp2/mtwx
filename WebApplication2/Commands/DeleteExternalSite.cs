using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class DeleteExternalSite : DeleteCommand, IRequest<int>
    {
        public DeleteExternalSite(int id) : base(id)
        {
        }
    }

    public class DeleteExternalSiteHandler : SqlHandlerBase<DeleteExternalSite, int>
    {
        public DeleteExternalSiteHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(DeleteExternalSite message)
        {
            return @"DELETE FROM ApplicationUserExternalSite WHERE ExternalSiteId = @Id;
                        DELETE FROM ApplicationRoleExternalSite WHERE ExternalSiteId = @Id;
                        DELETE FROM ExternalSite WHERE Id = @Id; ";
        }
    }
}