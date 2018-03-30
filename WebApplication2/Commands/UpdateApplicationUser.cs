using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class UpdateApplicationUser : IRequest<int>
    {
        public int Id { get; }
        public string Email { get; }
        public string Password { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string ModifiedBy { get; }

        public UpdateApplicationUser(int id, string email, string password, string firstName, string lastName, string modifiedBy)
        {
            Id = id;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            ModifiedBy = modifiedBy;
        }
    }

    public class UpdateApplicationUserHandler : SqlHandlerBase<UpdateApplicationUser, int>
    {
        public UpdateApplicationUserHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(UpdateApplicationUser message)
        {
            return @"UPDATE [dbo].[ApplicationUser]
                       SET [Email] = @Email
                            , [Password] = @Password
                          ,[FirstName] = @FirstName
                          ,[LastName] = @LastName
                        , [ModifiedBy] = @ModifiedBy
                        , [ModifiedDate] = CURRENT_TIMESTAMP
                     WHERE Id = @Id; ";
        }
    }
}