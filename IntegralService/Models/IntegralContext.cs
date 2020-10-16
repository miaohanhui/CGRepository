using IntegralService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegralService
{
    public class IntegralContext : DbContext
    {
        
        public IntegralContext(DbContextOptions<IntegralContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
           //这里可以写数据库连接信息
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {           
            
            base.OnModelCreating(modelBuilder);
        }


    }
}
