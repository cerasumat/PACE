using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using PACE.client.Aop;
using PACE.Utility;

namespace PACE.api.test
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional }
			);

			config.Filters.Add(new PaceFilterAttribute());
        }
    }
}
