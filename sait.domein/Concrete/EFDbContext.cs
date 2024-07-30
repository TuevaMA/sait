using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sait.domein.Entities;
using System.Data.Entity;

namespace sait.domein.Concrete
{
   public class EFDbContext : DbContext
    {
        public DbSet<Clothe> Clothes { get; set; }
    }
}
