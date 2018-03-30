using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetApplicationRoleList : IRequest<IEnumerable<ApplicationRoleEntity>>
    {
    }
    public class GetApplicationRoleListHandler : SqlHandlerBase<GetApplicationRoleList, IEnumerable<ApplicationRoleEntity>>
    {
        public GetApplicationRoleListHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(DataConnectionType.AppReadWrite, connectionFactory)
        {
        }

        protected override string GetSql(GetApplicationRoleList message)
        {
            return @"SELECT 
                        [Id]
                        ,[RoleName]
                        ,[Description]
                      FROM 
                        [dbo].[ApplicationRole]; ";
        }
    }
}