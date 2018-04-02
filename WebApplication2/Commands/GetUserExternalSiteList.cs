using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetUserExternalSiteList : IRequest<IEnumerable<ExternalSiteEntity>>
    {
        public int ApplicationUserId { get; }

        public GetUserExternalSiteList(int applicationUserId)
        {
            ApplicationUserId = applicationUserId;
        }
    }

    public class GetUserExternalSiteListHandler : SqlHandlerBase<GetUserExternalSiteList, IEnumerable<ExternalSiteEntity>>
    {
        public GetUserExternalSiteListHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetUserExternalSiteList message)
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
                  JOIN  [dbo].[ApplicationUserExternalSite] aues ON
                        es.[Id] = aues.[ExternalSiteId]
                  WHERE
                        aues.[ApplicationUserId] = @ApplicationUserId; ";
        }
    }
}