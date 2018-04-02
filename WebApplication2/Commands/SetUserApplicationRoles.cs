using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class SetUserApplicationRoles : IRequest<int>
    {
        public int ApplicationUserId { get; }
        public IEnumerable<int> Roles { get; }

        public SetUserApplicationRoles(int applicationUserId, IEnumerable<int> roles)
        {
            ApplicationUserId = applicationUserId;
            Roles = roles;
        }
    }

    public class SetUserApplicationRolesHandler : SqlHandlerBase<SetUserApplicationRoles, int>
    {
        public SetUserApplicationRolesHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(SetUserApplicationRoles message)
        {
            return string.Empty;
        }

        public override Task<int> Handle(SetUserApplicationRoles message, CancellationToken cancellationToken)
        {
            const string deleteCmd = "DELETE FROM ApplicationUserApplicationRole WHERE UserId = @ApplicationUserId; ";
            const string insertCmd = "INSERT INTO ApplicationUserApplicationRole VALUES (@ApplicationUserId, @ApplicationRoleId); ";

            return Task.Run(() =>
            {
                using (var cn = ConnectionFactory(ConnectionType))
                {
                    cn.Execute(deleteCmd, message);

                    // execute the insert command
                    var items = message.Roles.Select(x =>
                        new {message.ApplicationUserId, ApplicationRoleId = x.ToString()});
                    return cn.Execute(insertCmd, items);
                }
            }, cancellationToken);
        }
    }
}