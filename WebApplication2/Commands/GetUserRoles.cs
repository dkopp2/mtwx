using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetUserRoles : IRequest<IEnumerable<ApplicationRoleEntity>>
    {
        public string Email { get; }

        public GetUserRoles(string email)
        {
            Email = email;
        }
    }
    
    public class GetUserRolesHandler : SqlHandlerBase<GetUserRoles, IEnumerable<ApplicationRoleEntity>>
    {
        public GetUserRolesHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(DataConnectionType.AppReadWrite, connectionFactory)
        {
        }

        protected override string GetSql(GetUserRoles message)
        {
            return @"SELECT
                        ar.[Id]
                        ,[RoleName]
                        ,[Description]
                    FROM
                        [dbo].[ApplicationRole] ar
                    JOIN[dbo].[ApplicationUserApplicationRole] auar ON
                        ar.Id = auar.RoleId
                    JOIN[dbo].[ApplicationUser] au ON
                        au.Id = auar.UserId
                    WHERE
                        au.Email = @Email;";
        }
    }
}