using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GifService.Models
{
    [Table("Gif")]
    public class Gif
    {       
        [Key]
        [Required]
        [Column("ID")]
        public int GifId { set; get; }

        [Required]
        public string GifCode { set; get; }

        [Required]
        public string GifName { set; get; }
    }

    public enum GifType
    {
        RegistGif = 1
    }
}
