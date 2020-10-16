using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GifService.Models
{
    [Table("UserGif")]
    public class UserGif
    {
        [Key]
        [Required]
        [Column("ID")]
        public int UserGifId { set; get; }

        [Required]
        public int UserId { set; get; }

        [Required]
        public int GifId { set; get; }

        public DateTime GetDate { set; get; }

        public DateTime DeadLine { set; get; }


    }
}
