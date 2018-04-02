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
    public abstract class DeleteCommand
    {
        public int Id { get; }

        protected DeleteCommand(int id)
        {
            Id = id;
        }
    }
    public abstract class CreateCommand
    {
        public string CreatedBy { get; }

        protected CreateCommand(string createdBy)
        {
            CreatedBy = createdBy;
        }
    }

    public abstract class UpdateCommand
    {
        public int Id { get; }
        public string ModifiedBy { get; }

        public UpdateCommand(int id, string modifiedBy)
        {
            Id = id;
            ModifiedBy = modifiedBy;
        }
    }

    public class CreateApplicationUser : IRequest<ApplicationUserEntity>
    {
        public string Email { get; }
        public string Password { get; }
        public string FirstName { get; }
        public string LastName  { get; }
        public string CreatedBy { get; }

        public CreateApplicationUser(string email, string password, string firstName,
            string lastName, string createdBy)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            CreatedBy = createdBy;
        }
    }

    public class CreateApplicationUserHandler : SqlHandlerBase<CreateApplicationUser, ApplicationUserEntity>
    {
        public CreateApplicationUserHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(CreateApplicationUser message)
        {
            return @"

                     DECLARE @NewId INT

                    INSERT INTO [dbo].[ApplicationUser]
                               ([Email]
                                , [Password]
                                , [FirstName]
                                , [LastName]
                                , [CreatedBy]
                                , [CreateDate]
                                , [ModifiedBy]
                                , [ModifiedDate])
                         VALUES
                               ( @Email
                                , @Password
                                , @FirstName
                                , @LastName
                                , @CreatedBy
                                , CURRENT_TIMESTAMP
                                , @CreatedBy
                                , CURRENT_TIMESTAMP); 

                    SELECT 
                        @NewId = CAST(SCOPE_IDENTITY() as int);

                        SELECT 
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
                            au.[Id] = @NewId;";
        }

        public override Task<ApplicationUserEntity> Handle(CreateApplicationUser message, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (var cn = ConnectionFactory(ConnectionType))
                {
                    var cmd = GetSql(message);
                    return cn.Query<ApplicationUserEntity>(cmd, message).Single();
                }
            }, cancellationToken);
        }
    }

}