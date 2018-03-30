using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class UpdateApplicationRole : IRequest<int>
    {
        public int Id { get; }
        public string RoleName { get; }
        public string Description { get; }
        public string ModifiedBy { get; }

        public UpdateApplicationRole(int id, string roleName, string description, string modifiedBy)
        {
            Id = id;
            RoleName = roleName;
            Description = description;
            ModifiedBy = modifiedBy;
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