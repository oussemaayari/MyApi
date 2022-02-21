using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MyApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "WithActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,                    
                    Param = RouteParameter.Optional
                });
        }
    }
}
