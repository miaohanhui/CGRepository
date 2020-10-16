using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    [Table("RoleUrl")]
    public class RoleUrl
    {
        [Key]
        public int ID { set; get; }

        [Required]
        public int RoleId { set; get; }

        [Required]
        public string RoleName { set; get; }

        [Required]
        public string Url { set; get; }
    }
}
