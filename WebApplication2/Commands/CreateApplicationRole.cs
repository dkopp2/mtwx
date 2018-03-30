using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class CreateApplicationRole : IRequest<int>
    {
        public string RoleName { get;  }
        public string Description { get; }
        public string CreatedBy { get; }

        public CreateApplicationRole(string roleName, string description, string createdBy)
        {
            RoleName = roleName;
            Description = description;
            CreatedBy = createdBy;
        }
    }

    public class CreateApplicationRoleHandler : SqlHandlerBase<CreateApplicationRole, int>
    {
        public CreateApplicationRoleHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(CreateApplicationRole message)
        {
            return @"INSERT INTO [dbo].[ApplicationRole]
                           ([RoleName]
                           ,[Description]
                                , [CreatedBy]
                                , [CreateDate]
                                , [ModifiedBy]
                                , [ModifiedDate]
                            )
                     VALUES
                           (@RoleName
                           ,@Description
                                , @CreatedBy
                                , CURRENT_TIMESTAMP
                                , @CreatedBy
                                , CURRENT_TIMESTAMP); ";
        }
    }
}