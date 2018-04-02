using System;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetExternalSite : IRequest<ExternalSiteEntity>
    {
        public int Id { get; }

        public GetExternalSite(int id)
        {
            Id = id;
        }
    }

    public class GetExternalSiteHandler : SqlHandlerBase<GetExternalSite, ExternalSiteEntity>
    {
        public GetExternalSiteHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetExternalSite message)
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
                    [dbo].[ExternalSite]
                    WHERE
                        [Id] = @Id; ";
        }
    }
}