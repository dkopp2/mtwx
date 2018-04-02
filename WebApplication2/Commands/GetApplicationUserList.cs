using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetApplicationUserList : IRequest<IEnumerable<ApplicationUserEntity>>
    {
        
    }

    public class GetApplicationUserListHandler : SqlHandlerBase<GetApplicationUserList, IEnumerable<ApplicationUserEntity>>
    {
        public GetApplicationUserListHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetApplicationUserList message)
        {
            return @"SELECT 
                            [Id]
                                ,[Email]
                                , [Password]
                                , [FirstName]
                                , [LastName]
                            FROM 
                            [dbo].[ApplicationUser] au;";
        }
    }
}