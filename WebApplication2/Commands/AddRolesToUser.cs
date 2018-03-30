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
    public class AddRolesToUser : IRequest<int>
    {
        public List<int> RoleList { get; }
        public int UserId { get; }

        public AddRolesToUser(int userId, List<int> roleList)
        {
            UserId = userId;
            RoleList = roleList;
        }
    }

    public class AddRolesToUserHandler : SqlHandlerBase<AddRolesToUser, int>
    {
        private class UserRole
        {
            public int UserId { get; set; }
            public int RoleId { get; set; }
        }

        public AddRolesToUserHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(AddRolesToUser message)
        {
            return "INSERT INTO ApplicationUserApplicationRole (UserId, RoleId) VALUES (@UserId, @RoleId)";
        }

        public override Task<int> Handle(AddRolesToUser message, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var cmd = GetSql(message);
                var items = message.RoleList.Select(r => new UserRole {RoleId = r, UserId = message.UserId }).ToList();

                using (var cn = ConnectionFactory(ConnectionType))
                {
                    return cn.Execute(cmd, items);
                }
            }, cancellationToken);
        }
    }
}