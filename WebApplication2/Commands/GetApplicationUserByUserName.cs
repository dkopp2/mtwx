using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetApplicationUserByUserName : IRequest<ApplicationUserEntity>
    {
        public string UserName { get; }

        public GetApplicationUserByUserName(string userName)
        {
            UserName = userName;
        }
    }

    public class GetApplicationUserByUserNameHandler : SqlHandlerBase<GetApplicationUserByUserName, ApplicationUserEntity>
    {
        public GetApplicationUserByUserNameHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(DataConnectionType.AppReadWrite, connectionFactory)
        {
        }

        protected override string GetSql(GetApplicationUserByUserName message)
        {
            return @"SELECT 
                            [Id]
                                , [Email]
                                , [Password]
                                , [FirstName]
                                , [LastName]
                        FROM 
                            [dbo].[ApplicationUser] au
                        WHERE
                            au.[Email] = @Email; ";
        }
    }

}