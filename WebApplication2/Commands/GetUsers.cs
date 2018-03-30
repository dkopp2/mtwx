using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetUsers : IRequest<IEnumerable<ApplicationUserEntity>>
    {
        
    }

    public class GetUsersHandler : SqlHandlerBase<GetUsers, IEnumerable<ApplicationUserEntity>>
    {
        public GetUsersHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetUsers message)
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