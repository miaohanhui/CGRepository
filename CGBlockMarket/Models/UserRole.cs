using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public int ID { set; get; }

        [Required]
        [ForeignKey("UserId")]
        public User User { set; get; }

        [Required]
        public int RoleId { set; get; }


    }
}
