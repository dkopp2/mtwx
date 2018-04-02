using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class UpdateApplicationRole : UpdateCommand, IRequest<int>
    {
        public string RoleName { get; }
        public string Description { get; }

        public UpdateApplicationRole(int id, string roleName, string description, string modifiedBy) : base(id, modifiedBy)
        {
            RoleName = roleName;
            Description = description;
        }
    }
    public class UpdateApplicationRoleHandler : SqlHandlerBase<UpdateApplicationRole, int>
    {
        public UpdateApplicationRoleHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(UpdateApplicationRole message)
        {
            return @"UPDATE 
                        [dbo].[ApplicationRole]
                    SET 
                        [RoleName] = @RoleName
                        ,[Description] = @Description
                        , [ModifiedBy] = @ModifiedBy
                        , [ModifiedDate] = CURRENT_TIMESTAMP
                    WHERE
                        [Id] = @Id; ";
        }
    }
}