using System;
using System.Data.Entity;

using GeocachingExercise.Models;

namespace GeocachingExercise.Persistence.EF
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<GeocacheContext>
    {
        private static Random random = new Random();

        protected override void Seed(GeocacheContext context)
        {
            base.Seed(context);
            
            // Seed 15 valid geocache objects
            for(int ii = 1; ii <= 10; ii++)
            {
                Geocache cache = new Geocache
                {
                    Name = string.Format("Geocache {0}", ii),
                    Coordinate = new Coordinate(RandomLatitude(), RandomLongitude())
                };

                context.Geocaches.Add(cache);
            }

            context.SaveChanges();
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