using GifService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GifService
{
    public class GifContext:DbContext
    {
        public GifContext(DbContextOptions<GifContext> options) : base(options)
        {
            
        }
    
        public DbSet<Gif> Gifs { set; get; }
        public DbSet<UserGif> UserGifs { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          
            base.OnModelCreating(modelBuilder);
        }
    }
}
