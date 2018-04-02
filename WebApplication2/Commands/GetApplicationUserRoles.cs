using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{

    public class GetApplicationUserRoles : IRequest<IEnumerable<ApplicationRoleEntity>>
    {
        public int UserId { get; }

        public GetApplicationUserRoles(int userId)
        {
            UserId = userId;
        }
    }

    public class GetApplicationUserRolesHandler : SqlHandlerBase<GetApplicationUserRoles, IEnumerable<ApplicationRoleEntity>>
    {
        public GetApplicationUserRolesHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(DataConnectionType.AppReadWrite, connectionFactory)
        {
        }

        protected override string GetSql(GetApplicationUserRoles message)
        {
            return @"SELECT 
	                    ar.Id,
	                    ar.RoleName,
	                    ar.[Description]
                    FROM
	                    ApplicationRole ar
                    JOIN ApplicationUserApplicationRole auar ON
	                    auar.RoleId = ar.Id
                    WHERE
	                    auar.UserId = @UserId; ";
        }
    }
}