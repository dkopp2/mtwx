using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{

    public class GetApplicationUser : IRequest<ApplicationUserEntity>
    {
        public int Id { get; }

        public GetApplicationUser(int id)
        {
            Id = id;
        }
    }

    public class GetApplicationUserHandler : SqlHandlerBase<GetApplicationUser, ApplicationUserEntity>
    {
        public GetApplicationUserHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetApplicationUser message)
        {
            return @"SELECT 
                            [Id]
                                , [Email]
                                , [Password]
                                , [FirstName]
                                , [LastName]
                          ,[CreateDate]
                          ,[CreatedBy]
                          ,[ModifiedDate]
                          ,[ModifiedBy]
                        FROM 
                            [dbo].[ApplicationUser] au
                        WHERE
                            au.[Id] = @Id; ";
        }
    }
}