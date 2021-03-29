using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evlilikTeklif.Models
{
    public class evlilikTeklifContext:DbContext
    {
        public DbSet<questions> Questions { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("DataSource=teklifDb");
          
        }

    }
}
