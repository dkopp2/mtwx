using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class DeleteApplicationUser : IRequest<int>
    {
        public int Id { get; }

        public DeleteApplicationUser(int id)
        {
            Id = id;
        }
    }

    public class DeleteApplicationUserHandler : SqlHandlerBase<DeleteApplicationUser, int>
    {
        public DeleteApplicationUserHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(DeleteApplicationUser message)
        {
            return @"
                DELETE FROM ApplicationUserApplicationRole WHERE UserId = @Id;
                DELETE FROM ApplicationUserExternalSite WHERE ApplicationUserId = @Id;
                DELETE FROM ApplicationUser WHERE Id = @Id;
            ";
        }
    }

}