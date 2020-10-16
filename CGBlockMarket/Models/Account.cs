using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    /*系统用户账户*/
    [Table("Account")]
    public class Account
    {
        [Required]
        public int AccountId { set; get; }

        [Required]
        public string AccountNum { set; get; }

        [ForeignKey("UserId")]
        public User User { set; get; }

        public int Integral { set; get; }

        public int IsFirstCreate { set; get; }

        public List<UserGif> UserGifs { set; get; }
    }
}
