using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class CreateApplicationRole : IRequest<ApplicationRoleEntity>
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

    public class CreateApplicationRoleHandler : SqlHandlerBase<CreateApplicationRole, ApplicationRoleEntity>
    {
        public CreateApplicationRoleHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(CreateApplicationRole message)
        {
            return @"
                    DECLARE @NewId INT

                    INSERT INTO [dbo].[ApplicationRole]
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
                                , CURRENT_TIMESTAMP);

                    SELECT 
                        @NewId = CAST(SCOPE_IDENTITY() as int);

                    SELECT 
                        [Id]
                        ,[RoleName]
                        ,[Description]
                          ,[CreateDate]
                          ,[CreatedBy]
                          ,[ModifiedDate]
                          ,[ModifiedBy]
                      FROM 
                        [dbo].[ApplicationRole]
                        WHERE
                            [Id] = @NewId; ";
        }

        public override Task<ApplicationRoleEntity> Handle(CreateApplicationRole message, CancellationToken cancellationToken)
        {
             return Task.Run(() =>
            {
                using (var cn = ConnectionFactory(ConnectionType))
                {
                    var cmd = GetSql(message);
                    return cn.Query<ApplicationRoleEntity>(cmd, message).Single();
                }
            }, cancellationToken);
        }
    }
}