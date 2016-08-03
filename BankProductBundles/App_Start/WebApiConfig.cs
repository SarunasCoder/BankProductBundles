using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BankProductBundles
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
									routeTemplate: "api/{controller}/{id}",
									defaults: new { id = RouteParameter.Optional }
							);

						config.Routes.MapHttpRoute(
											name: "2ParamApi",
											routeTemplate: "api/{controller}/{userId}/{productId}",
											defaults: new { userId = RouteParameter.Optional, productId = RouteParameter.Optional }
									);

						config.Routes.MapHttpRoute(
									name: "3ParamApi",
									routeTemplate: "api/{controller}/{age}/{student}/{income}",
									defaults: new { age = RouteParameter.Optional, student = RouteParameter.Optional, income = RouteParameter.Optional }
									
							);

            
        }
    }
}
