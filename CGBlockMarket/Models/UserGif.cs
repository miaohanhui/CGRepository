using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    public class UserGif
    {
        public int ID { set; get; }

        [ForeignKey("AcountId")]
        public Account Account { set; get; }

        [ForeignKey("GifId")]
        public Gif Gif { set; get; }

        public DateTime GetDate { set; get; }

        public DateTime DeadLine { set; get; }


    }
}
