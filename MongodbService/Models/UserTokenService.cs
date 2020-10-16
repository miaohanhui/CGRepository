using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongodbService.Models
{
    public class UserTokenService:BaseService<UserToken>
    {
        public UserTokenService(IConfiguration config) : base(config, nameof(UserToken))
        {

        }
    }
}
