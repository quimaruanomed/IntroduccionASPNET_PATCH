using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HolaMundoChangeNameApi.Models
{
    public class ChangeNameCTX : DbContext
    {
        public ChangeNameCTX(DbContextOptions<ChangeNameCTX> options): base(options)
        {

        }
        public DbSet<ChangeName> ChangeName { get; set; }
    }
}
