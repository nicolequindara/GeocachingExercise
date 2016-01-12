using System.Data.Entity;
using GeocachingExercise.Models;

namespace GeocachingExercise.Persistence.EF
{
    public class GeocacheContext : DbContext
    {
        public GeocacheContext() : base("GeocacheContext")
        {
        }

        public DbSet<Geocache> Geocaches { get; set; }
    }
}