using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetApplicationRoleExternalSiteList : IRequest<IEnumerable<ExternalSiteEntity>>
    {
        public int ApplicationRoleId { get; }

        public GetApplicationRoleExternalSiteList(int applicationRoleId)
        {
            ApplicationRoleId = applicationRoleId;
        }
    }

    public class GetApplicationRoleExternalSiteListHandler : SqlHandlerBase<GetApplicationRoleExternalSiteList,
            IEnumerable<ExternalSiteEntity>>
    {
        public GetApplicationRoleExternalSiteListHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetApplicationRoleExternalSiteList message)
        {
            return @"SELECT [Id]
                        , [Name]
                        , [Description]
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
                      ,[ModifiedBy]
                  FROM 
                        [dbo].[ExternalSite] es
                  JOIN  [dbo].[ApplicationRoleExternalSite] aues ON
                        es.[Id] = aues.[ExternalSiteId]
                  WHERE
                        aues.[ApplicationRoleId] = @ApplicationRoleId; ";
        }
    }
}