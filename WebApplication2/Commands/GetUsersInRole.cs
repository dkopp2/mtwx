using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetUsersInRole : IRequest<IEnumerable<ApplicationUserEntity>>
    {
        public string RoleName { get; }

        public GetUsersInRole(string roleName)
        {
            RoleName = roleName;
        }
    }

    public class GetUsersInRoleHandler : SqlHandlerBase<GetUsersInRole, IEnumerable<ApplicationUserEntity>>
    {
        public GetUsersInRoleHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(DataConnectionType.AppReadWrite, connectionFactory)
        {
        }

        protected override string GetSql(GetUsersInRole message)
        {
            return @"SELECT 
                            au.[Id]
                                , [Email]
                                , [Password]
                                , [FirstName]
                                , [LastName]
                        FROM 
                            [dbo].[ApplicationUser] au
                        JOIN [dbo].[ApplicationUserApplicationRole] auar ON
	                        auar.UserId = au.Id
                        JOIN [dbo].[ApplicationRole] ar ON
	                        ar.Id = auar.RoleId
                        WHERE
                            au.[RoleName] = @RoleName;";
        }
    }
}