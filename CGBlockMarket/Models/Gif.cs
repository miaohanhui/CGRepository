using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    [Table("Gif")]
    public class Gif
    {
        [Required]
        public int ID { set; get; }

        [Required]
        public string GifCode { set; get; }

        [Required]
        public string GifName { set; get; }

        public List<UserGif> UserGifs { set; get; }
    }
}
