using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    /*系统用户*/
    [Table("User")]
    public class User
    {
        [Required]     
        public int UserId { set; get; }

        [Required]
        [MaxLength(20)]
        [DisplayName("用户账号")]
        public string UserNum { set; get; }

        [Required]
        public string Password { set; get; }

        [Required]
        [MaxLength(20)]
        [DisplayName("用户名称")]
        public string UserName { set; get; }

        [MaxLength(11)]
        [MinLength(11)]
        [MaybeNull]   
        [DisplayName("用户手机号")]
        public string PhoneNum { set; get; }

        [MaybeNull]
        [EmailAddress]
        [DisplayName("用户邮箱")]
        public string Email { set; get; }

        public int IsDeleted { set; get; }

        [MaybeNull]
        public DateTime CreatedDate{ set; get; }

        public List<Account> Accounts { set; get; }
    }
}
