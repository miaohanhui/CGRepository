using Microsoft.Extensions.DependencyInjection;
using CGBlockMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CGBlockMarket.Permission
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _provider;
        public PermissionHandler(IServiceProvider provider)
        {
            _provider = provider;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            string url = "";
            string method = "";
            if (context.Resource is RouteEndpoint)
            {
                var route = context.Resource as RouteEndpoint;
                if (route.RoutePattern.PathSegments.Count > 0)
                {
                    url = $"{route.RoutePattern.Defaults["controller"]?.ToString()?.ToLower()}/{route.RoutePattern.Defaults["action"]?.ToString()?.ToLower()}";
                }
                else
                {
                    url = route.RoutePattern.RawText?.ToLower();
                }
            }
            else
            {
                var filter = context.Resource as AuthorizationFilterContext;
                url = filter?.HttpContext?.Request?.Path.Value?.ToString();
                method = filter?.HttpContext?.Request?.Method;
            }

            //因为asp.net core 的AddDbContext方法默认将 DBContext注入为Scope类型，在Singleton注入的服务中使用Scope类型的注入要通过ServiceProvider的GetService方法
            CGBlockContext _context = _provider.GetService<CGBlockContext>();
            var roleUrls = _context.RoleUrls.ToList();


            var isAuthenticated = context?.User?.Identity.IsAuthenticated;
            if (isAuthenticated.HasValue && isAuthenticated.Value)
            {
                if (roleUrls.Where(c => c.Url.ToLower() == url.ToLower()).Count() > 0)
                {
                    var value = context.User.Claims.SingleOrDefault(c => c.Type == requirement.ClaimType)?.Value;
                    if (roleUrls.Where(c => c.Url == url && c.RoleName == value).Count() > 0)//该用户有该Route的权限
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
                else
                {
                    context.Fail();
                }
            }

            return Task.CompletedTask;
        }
    }
}
