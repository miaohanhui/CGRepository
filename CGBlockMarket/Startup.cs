using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CGBlockMarket.Models;
using CGBlockMarket.Permission;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CGBlockMarket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var audience = Configuration.GetSection("Audience")["Audience"];
            var secret = Configuration.GetSection("Audience")["Secret"];
            var issuer = Configuration.GetSection("Audience")["Issuer"];
            //对称加密
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            //签署凭证
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            //var userPermissions = new List<UserPermission>() {
            //        new UserPermission{ Url = "/home/index", Name="admin111"},
            //        //new UserPermission{ Url = "/", UserName="miaohanhui"},
            //    };//实际应该从数据库权限表读取角色对应可访问的url


            var permissionReqirement = new PermissionRequirement("/home/denied", ClaimTypes.Role, issuer, signingCredentials, audience, TimeSpan.FromSeconds(1000));

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Permission", policy =>
                {
                    policy.Requirements.Add(permissionReqirement);
                });
            })
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                };
                    
            });
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();          
            services.AddSingleton(permissionReqirement);
            services.AddHttpContextAccessor();
            services.AddSession();
            //数据库链接字符串
            string connectionString = Configuration.GetSection("ConnectionString")["Connection"];
            services.AddDbContext<CGBlockContext>(options =>
            {
                options.UseSqlServer(connectionString, m => 
                    m.MigrationsAssembly("CGBlockMarket"));
            });


            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
            });
            //services.AddHttpClient("RedisService", option=> {
            //    option.BaseAddress = new Uri("http://localhost:6001/api/redis");
            //});
            
            services.AddHttpClient<RedisService>();
            services.AddHttpClient<MongodbService>();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            //Add JWToken to all incoming HTTP Request Header
            //中间件
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });


            app.UseAuthentication();
            app.UseRouting();           
            app.UseAuthorization();
            
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}"
                );
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            
        }
    }
}
