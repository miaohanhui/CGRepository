using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongodbService.Models
{
    public class UserToken : BaseModel
    {
        public string UserNum { set; get; }
        public string Token { set; get; }
    }
}
