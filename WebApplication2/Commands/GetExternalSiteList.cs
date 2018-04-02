using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetExternalSiteList : IRequest<IEnumerable<ExternalSiteEntity>>
    {
        
    }

    public class GetExternalSiteListHandler : SqlHandlerBase<GetExternalSiteList, IEnumerable<ExternalSiteEntity>>
    {
        public GetExternalSiteListHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetExternalSiteList message)
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
                  FROM [dbo].[ExternalSite]; ";
        }
    }
}