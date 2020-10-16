using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    /*系统用户角色*/
    [Table("Role")]
    public class Role
    {
        [Required]
        public int RoleId { set; get; }

        [Required]
        public string RoleCode { set; get; }

        [Required]
        public string RoleName { set; get; }
    }
}
