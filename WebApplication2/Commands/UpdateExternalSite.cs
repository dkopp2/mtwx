using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;

namespace Mtwx.Web.Commands
{
    public class UpdateExternalSite : UpdateCommand, IRequest<int>
    {
        public string Name { get; }
        public string Description { get; }
        public string FormId { get; }
        public string Href { get; }
        public string LoginAction { get; }
        public string SiteUserId { get; }
        public string SitePassword { get; }
        public string FormUserIdField { get; }
        public string FormPasswordField { get; }

        public UpdateExternalSite(string name, string description, string formId, string href, string loginAction, string siteUserId,
            string sitePassword, string formUserIdField, string formPasswordField, int id, string modifiedBy) : base(id, modifiedBy)
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

    public class UpdateExternalSiteHandler : SqlHandlerBase<UpdateExternalSite, int>
    {
        public UpdateExternalSiteHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(UpdateExternalSite message)
        {
            return @"UPDATE [dbo].[ExternalSite]
                       SET 
                            [Name] = @Name
                            , [Description] = @Description
                            , [FormId] = @FormId
                          , [Href] = @Href
                          , [LoginAction] = @LoginAction
                          , [SiteUserId] = @SiteUserId
                          , [SitePassword] = @SitePassword
                          , [FormUserIdField] = @FormUserIdField
                          , [FormPasswordField] = @FormPasswordField
                          , [ModifiedDate] = CURRENT_TIMESTAMP
                          , [ModifiedBy] = @ModifiedBy
                     WHERE 
                            Id = @Id; ";
        }
    }
}