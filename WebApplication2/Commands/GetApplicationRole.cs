using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetApplicationRole : IRequest<ApplicationRoleEntity>
    {
        public int RoleId { get; }

        public GetApplicationRole(int roleId)
        {
            RoleId = roleId;
        }
    }

    public class GetApplicationRoleHandler : SqlHandlerBase<GetApplicationRole, ApplicationRoleEntity>
    {
        public GetApplicationRoleHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetApplicationRole message)
        {
            return @"SELECT 
                        [Id]
                        ,[RoleName]
                        ,[Description]
                          ,[CreateDate]
                          ,[CreatedBy]
                          ,[ModifiedDate]
                          ,[ModifiedBy]
                      FROM 
                        [dbo].[ApplicationRole]
                        WHERE
                            [Id] = @RoleId; ";
        }
    }
}