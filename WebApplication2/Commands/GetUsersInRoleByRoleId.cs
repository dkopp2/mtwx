using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetUsersInRoleByRoleId : IRequest<IEnumerable<ApplicationUserEntity>>
    {
        public int RoleId { get; }

        public GetUsersInRoleByRoleId(int roleId)
        {
            RoleId = roleId;
        }
    }

    public class GetUsersInRoleByRoleIdHandler : SqlHandlerBase<GetUsersInRoleByRoleId, IEnumerable<ApplicationUserEntity>>
    {
        public GetUsersInRoleByRoleIdHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(DataConnectionType.AppReadWrite, connectionFactory)
        {
        }

        protected override string GetSql(GetUsersInRoleByRoleId message)
        {
            return @"SELECT 
                            au.[Id]
                                ,[Email]
                                , [Password]
                                , [FirstName]
                                , [LastName]
                        FROM 
                            [dbo].[ApplicationUser] au
                        JOIN [dbo].[ApplicationUserApplicationRole] auar ON
	                        auar.UserId = au.Id
                        WHERE
                            auar.[RoleId] = @RoleId;";
        }
    }

}