using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class DeleteApplicationRole : IRequest<int>
    {
        public int Id { get; }

        public DeleteApplicationRole(int id)
        {
            Id = id;
        }
    }

    public class DeleteApplicationRoleHandler : SqlHandlerBase<DeleteApplicationRole, int>
    {
        public DeleteApplicationRoleHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(DeleteApplicationRole message)
        {
            return @"
                    DELETE FROM [dbo].[ApplicationUserApplicationRole] WHERE RoleId = @Id;
                    DELETE FROM [dbo].[ApplicationRole] WHERE Id = @Id;";
        }
    }
}