using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IntegralService.Models
{
    /*系统用户账户*/
    [Table("Account")]
    public class Account
    {
        [Required]
        public int AccountId { set; get; }

        [Required]
        public string AccountNum { set; get; }

        public int UserId { set; get; }

        public int Integral { set; get; }

        public int IsFirstCreate { set; get; }

    }
}
