using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetUserByEmail : IRequest<ApplicationUserEntity>
    {
        public string Email { get; }

        public GetUserByEmail(string email)
        {
            Email = email;
        }
    }

    public class GetUserByEmailHandler : SqlHandlerBase<GetUserByEmail, ApplicationUserEntity>
    {
        public GetUserByEmailHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetUserByEmail message)
        {
            return @"SELECT 
                            [Id]
                            ,[Email]
                            , [Password]
                            , [FirstName]
                            , [LastName]
                        FROM 
                            [dbo].[ApplicationUser] au
                        WHERE
                            au.Email = @Email; ";
        }
    }
}