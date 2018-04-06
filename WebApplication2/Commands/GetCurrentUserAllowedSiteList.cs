using System;
using System.Collections.Generic;
using System.Data;
using MediatR;
using Mtwx.Web.Data;
using Mtwx.Web.Entities;

namespace Mtwx.Web.Commands
{
    public class GetCurrentUserAllowedSiteList: IRequest<IEnumerable<ExternalSiteEntity>> {
        public string Email { get; }

        public GetCurrentUserAllowedSiteList(string email)
        {
            Email = email;
        }
    }

    public class
        GetCurrentUserAllowedSiteListHandler : SqlHandlerBase<GetCurrentUserAllowedSiteList,
            IEnumerable<ExternalSiteEntity>>
    {
        public GetCurrentUserAllowedSiteListHandler(Func<DataConnectionType, IDbConnection> connectionFactory) : base(connectionFactory)
        {
        }

        protected override string GetSql(GetCurrentUserAllowedSiteList message)
        {
            return @"SELECT DISTINCT
	                        es1.[Id]
                              ,es1.[Name]
                              ,es1.[Description]
                              ,es1.[FormId]
                              ,es1.[Href]
                              ,es1.[LoginAction]
                              ,es1.[SiteUserId]
                              ,es1.[SitePassword]
                              ,es1.[FormUserIdField]
                              ,es1.[FormPasswordField]
                              ,es1.[CreateDate]
                              ,es1.[CreatedBy]
                              ,es1.[ModifiedDate]
                              ,es1.[ModifiedBy]
                        FROM (

                        SELECT DISTINCT es.[Id]
                              ,es.[Name]
                              ,es.[Description]
                              ,es.[FormId]
                              ,es.[Href]
                              ,es.[LoginAction]
                              ,es.[SiteUserId]
                              ,es.[SitePassword]
                              ,es.[FormUserIdField]
                              ,es.[FormPasswordField]
                              ,es.[CreateDate]
                              ,es.[CreatedBy]
                              ,es.[ModifiedDate]
                              ,es.[ModifiedBy]
                          FROM [dbo].[ExternalSite] es
	                        JOIN ApplicationUserExternalSite auer ON
		                        auer.ExternalSiteId = es.Id
	                        JOIN ApplicationUser au ON
		                        au.Id = auer.ApplicationUserId
	                        WHERE
		                        au.Email= @Email
                        UNION ALL
                        SELECT DISTINCT es.[Id]
                              ,es.[Name]
                              ,es.[Description]
                              ,es.[FormId]
                              ,es.[Href]
                              ,es.[LoginAction]
                              ,es.[SiteUserId]
                              ,es.[SitePassword]
                              ,es.[FormUserIdField]
                              ,es.[FormPasswordField]
                              ,es.[CreateDate]
                              ,es.[CreatedBy]
                              ,es.[ModifiedDate]
                              ,es.[ModifiedBy]
                          FROM [dbo].[ExternalSite] es
	                        JOIN ApplicationRoleExternalSite arer ON
		                        arer.ExternalSiteId = es.Id
	                        JOIN ApplicationRole ar ON
		                        ar.Id = arer.ApplicationRoleId
	                        JOIN ApplicationUserApplicationRole auar ON
		                        auar.RoleId = ar.Id
	                        JOIN ApplicationUser au ON
		                        au.Id = auar.UserId
	                        WHERE
		                        au.Email= @Email) es1; ";
        }
    }
}