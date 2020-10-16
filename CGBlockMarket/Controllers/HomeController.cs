using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using CGBlockMarket.Models;
using CGBlockMarket.Permission;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using static CGBlockMarket.Permission.JWTToken;

namespace CGBlockMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly CGBlockContext _context;
        private readonly RedisService _redisService;
        private readonly MongodbService _mongodbService;
        private readonly PermissionRequirement _permissionRequirement;
        public HomeController(CGBlockContext context, RedisService redisService, MongodbService mongodbService, PermissionRequirement permissionReqirement)
        {
            _context = context;
            _redisService = redisService;
            _mongodbService = mongodbService;
            _permissionRequirement = permissionReqirement;
        }

        [Authorize(Policy = "Permission")]
        [HttpGet]
        public IActionResult Index()
        {
            List<Gif> gifs = _context.Gifs.ToList();
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            return View(gifs);
        }

        public IActionResult Regist()
        {
            return View();
        }

        public class PostUserMsg
        {
            public string value { set; get; }
            public string userMsg { set; get; }
        }

        [HttpPost]
        public async Task<IActionResult> OnRegist(User user)
        {
            if (ModelState.IsValid)
            {                           
                //请求redis服务
                var result = await _redisService.ServiceHttpGet("GetUserData?value=" + user.UserNum);
                if (result.IndexOf("error") < 0)
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        return Content("账号已被注册");
                    }
                    else
                    {                      
                        user.CreatedDate = DateTime.Now;

                        UserRole userrole = new UserRole();
                        userrole.RoleId = 2;
                        userrole.User = user;

                        _context.UserRoles.Add(userrole);
                        _context.SaveChanges();

                        PostUserMsg postUserMsg = new PostUserMsg();
                        postUserMsg.value = user.UserNum;
                        string usermsg = "userid:" + user.UserId + "|" + "usernum:" + user.UserNum;
                        postUserMsg.userMsg = usermsg;
                        var json = JsonConvert.SerializeObject(postUserMsg);

                        //账号信息保存到redis
                        result = await _redisService.ServiceHttpPost("SetUserData", json);//保存账号信息到redis
                        //通知消息队列
                        RabbitMqHelper.SendFanoutMsg(usermsg, "UserRegist");

                        var userRoles = _context.RoleUrls.Where(c => c.RoleId == 2).ToList();
                        List<Claim> claimList = new List<Claim>();
                        claimList.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claimList.Add(new Claim(ClaimTypes.Sid, user.UserNum));
                        foreach (var userRole in userRoles)
                        {
                            int roleId = userRole.RoleId;
                            Role role = _context.Roles.Where(c => c.RoleId == roleId).FirstOrDefault();

                            claimList.Add(new Claim(ClaimTypes.Role, role.RoleName));
                        }
                        claimList.Add(new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString()));

                        var claims = claimList.ToArray();

                        //ClaimsIdentity identity = new ClaimsIdentity("JwtIdentity");
                        //for (var i = 0; i < claims.Length; i++)
                        //{
                        //    identity.AddClaim(claims[i]);
                        //}
                        ////签发一个加密后的用户信息凭证，用来标识用户的身份
                        //await _httpContextAccessor.HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                        JwtToken token = JWTToken.BuildJwtToken(claims, _permissionRequirement);
                        HttpContext.Session.SetString("JWToken", token.access_Token);
                        return Redirect("/main/home/index");
                    }                 
                }
                else
                {
                    return Content("error请求失败");
                }             
            }
            else
            {
                return Content("数据不正确，注册失败");
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Denied()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> OnLogin(string UserNum, string Password)
        {
            var loginUser = _context.Users.Where(c => c.UserNum == UserNum & c.Password == Password).FirstOrDefault();
            if (loginUser == null)
            {
                return Content("账号或密码错误");
            }
            //从数据库查询用户密码是否正确
            else
            {
                HttpContext.Session.SetInt32("UserId", loginUser.UserId);
                List<UserRole> userRoles = _context.UserRoles.Where(c => c.User.UserId == loginUser.UserId).ToList();
                List<Claim> claimList = new List<Claim>();
                claimList.Add(new Claim(ClaimTypes.Name, loginUser.UserName));
                claimList.Add(new Claim(ClaimTypes.Sid, loginUser.UserNum));
                foreach (var userRole in userRoles)
                {
                    int roleId = userRole.RoleId;
                    Role role = _context.Roles.Where(c => c.RoleId == roleId).FirstOrDefault();

                    claimList.Add(new Claim(ClaimTypes.Role, role.RoleName));
                }
                claimList.Add(new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString()));

                var claims = claimList.ToArray();

                //ClaimsIdentity identity = new ClaimsIdentity("JwtIdentity");
                //for (var i = 0; i < claims.Length; i++)
                //{
                //    identity.AddClaim(claims[i]);
                //}
                ////签发一个加密后的用户信息凭证，用来标识用户的身份
                //await _httpContextAccessor.HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                JwtToken token = JWTToken.BuildJwtToken(claims, _permissionRequirement);
                HttpContext.Session.SetString("JWToken", token.access_Token);
                //return new JsonResult(token);//显示token
                await _mongodbService.ServiceHttpGet("SaveToken?usernum=" + UserNum + "&usertoken=" + token.access_Token);

                return Redirect("/main/home/index");
            }
        }
    }
}
