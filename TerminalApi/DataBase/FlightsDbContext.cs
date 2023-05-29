using Microsoft.EntityFrameworkCore;
using TerminalApi.Models;

namespace TerminalApi.DataBase
{
    public class FlightsDbContext : DbContext
    {
        public FlightsDbContext(DbContextOptions<FlightsDbContext> options) : base(options) 
        {

        }
       public virtual DbSet<Flight>? Flights { get; set; }

    }
}
