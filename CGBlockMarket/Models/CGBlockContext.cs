using CGBlockMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket
{
    public class CGBlockContext : DbContext
    {
        
        public CGBlockContext(DbContextOptions<CGBlockContext> options) : base(options)
        {

        }

        public DbSet<User> Users { set; get; }

        public DbSet<Role> Roles { set; get; }

        public DbSet<Account> Accounts { set; get; }

        public DbSet<Gif> Gifs { set; get; }

        public DbSet<UserGif> UserGifs { set; get; }

        public DbSet<UserRole> UserRoles { set; get; }

        public DbSet<RoleUrl> RoleUrls { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
           //这里可以写数据库连接信息
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<UserRole>().HasOne(m => m.User).WithMany().IsRequired(true);
            
            modelBuilder.Entity<Account>().HasOne(m => m.User).WithMany().IsRequired(true);
            modelBuilder.Entity<UserGif>().HasOne(m => m.Account).WithMany().IsRequired(true);
            modelBuilder.Entity<UserGif>().HasOne(m => m.Gif).WithMany().IsRequired(true);

            base.OnModelCreating(modelBuilder);
        }       
    }
}
