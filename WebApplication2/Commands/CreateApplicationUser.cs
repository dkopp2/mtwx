using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class CreateApplicationUser : IRequest<int>
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

    public class CreateApplicationUserHandler : SqlHandlerBase<CreateApplicationUser, int>
    {
        public CreateApplicationUserHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(CreateApplicationUser message)
        {
            return @"
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
                                , CURRENT_TIMESTAMP); ";
        }
    }

}