using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class CreateExternalSite : CreateCommand, IRequest<int>
    {
        public string Name { get; }
        public string Description { get; }
        public string FormId { get; }
        public string Href { get; }
        public string LoginAction { get;  }
        public string SiteUserId { get; }
        public string SitePassword { get; }
        public string FormUserIdField { get; }
        public string FormPasswordField { get; }

        public CreateExternalSite(string name, string description, string formId, string href, string loginAction, string siteUserId,
            string sitePassword,
            string formUserIdField, string formPasswordField, string createdBy): base(createdBy)
        {
            Name = name;
            Description = description;
            FormId = formId;
            Href = href;
            LoginAction = loginAction;
            SiteUserId = siteUserId;
            SitePassword = sitePassword;
            FormPasswordField = formPasswordField;
            FormUserIdField = formUserIdField;
        }
    }

    public class CreateExternalSiteHandler : SqlHandlerBase<CreateExternalSite, int>
    {
        public CreateExternalSiteHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(CreateExternalSite message)
        {
            return @"INSERT INTO [dbo].[ExternalSite]
                       ([Name]
                        ,[Description]
                        ,[FormId]
                        ,[Href]
                        ,[LoginAction]
                        ,[SiteUserId]
                        ,[SitePassword]
                        ,[FormUserIdField]
                        ,[FormPasswordField]
                        ,[CreateDate]
                        ,[CreatedBy]
                        ,[ModifiedDate]
                        ,[ModifiedBy])
                 VALUES
                       (
                        @Name
                        , @Description
                        , @FormId
                       ,@Href
                       ,@LoginAction
                       ,@SiteUserId
                       ,@SitePassword
                       ,@FormUserIdField
                       ,@FormPasswordField
                        , CURRENT_TIMESTAMP
                       ,@CreatedBy
                        , CURRENT_TIMESTAMP
                       ,@CreatedBy); ";
        }
    }
}