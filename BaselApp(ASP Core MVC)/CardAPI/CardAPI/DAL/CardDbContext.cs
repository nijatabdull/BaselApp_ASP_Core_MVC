using BaselFinalProjectApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardAPI.DAL
{
    public class CardDbContext : DbContext
    {
        public CardDbContext(DbContextOptions<CardDbContext> dbContextOptions) : base(dbContextOptions) { }

        public virtual DbSet<Card> Cards { get; set; }
    }
}
