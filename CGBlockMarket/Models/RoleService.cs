using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    public interface IRoleService
    {
        List<UserRole> GetUserRoleList();
        List<RoleUrl> GetRoleUrlList();
    }

    public class RoleService: IRoleService
    {
        private readonly IServiceProvider _provider;
        public RoleService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public List<UserRole> GetUserRoleList()
        {
            CGBlockContext _context = _provider.GetService<CGBlockContext>();
            return _context.UserRoles.ToList();
        }

        public List<RoleUrl> GetRoleUrlList()
        {
            CGBlockContext _context = _provider.GetService<CGBlockContext>();
            return _context.RoleUrls.ToList();
        }
    }
}
