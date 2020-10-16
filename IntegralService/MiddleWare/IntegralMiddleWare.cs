using IntegralService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegralService.MiddleWare
{
    public class IntegralMiddleWare
    {
        private readonly RequestDelegate _next;
        public IntegralMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            RabbitMqHelper.Open("UserRegist", AddIntgralToUserAccount);

            return _next(context);
        }

        private void AddIntgralToUserAccount(string UserNum)
        {
            /*
             * 查询用户的积分账户并奖励注册积分
             */
            Console.WriteLine($"用户是:{UserNum}");
        }
    }

    public static class IntegralMiddleWareExtend
    {
        public static IApplicationBuilder UseIntegralMiddleWare(this IApplicationBuilder app)
        {
            return app.UseMiddleware<IntegralMiddleWare>();
        }
    }
}
