using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class ClearUserRoles : IRequest<int>
    {
        public int UserId { get; }

        public ClearUserRoles(int userId)
        {
            UserId = userId;
        }
    }

    public class ClearUserRolesHandler : SqlHandlerBase<ClearUserRoles, int>
    {
        public ClearUserRolesHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(ClearUserRoles message)
        {
            return @"DELETE FROM
	                    ApplicationUserApplicationRole
                    WHERE
	                    UserId = @UserId; ";
        }
    }
}