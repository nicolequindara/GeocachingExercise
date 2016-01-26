namespace GeocachingExercise.Migrations
{
    using GeocachingExercise.Models;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Persistence.EF.GeocacheContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        private static Random random = new Random();

        protected override void Seed(GeocachingExercise.Persistence.EF.GeocacheContext context)
        {
            // Seed 15 valid geocache objects
            for (int ii = 1; ii <= 10; ii++)
            {
                Geocache cache = new Geocache
                {
                    Name = string.Format("Geocache {0}", ii),
                    Coordinate = new Coordinate(RandomLatitude(), RandomLongitude())
                };

                context.Geocaches.AddOrUpdate(cache);
            } 
        }


        private double RandomLatitude()
        {
            lock (random) // Random was not threadsafe
            {
                return -90.0 + random.NextDouble() * 180.0;
            }
        }

        private double RandomLongitude()
        {
            lock (random) // Random was not threadsafe
            {
                return -180.0 + random.NextDouble() * 360.0;
            }
        }
    }
}
