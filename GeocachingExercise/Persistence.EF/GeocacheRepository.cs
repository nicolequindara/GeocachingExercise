using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeocachingExercise.Models;

namespace GeocachingExercise.Persistence.EF
{
    public class GeocacheRepository : IGeocacheRepository, IDisposable
    {
        private GeocacheContext context;
        private bool disposed = false;

        public GeocacheRepository(GeocacheContext context)
        {
            this.context = context;
        }

        public IEnumerable<Geocache> All()
        {
            return context.Geocaches.ToList();
        }
        public Geocache Find(int? Id)
        {
            return context.Geocaches.Find(Id);
        }

        public void Create(Geocache cache)
        {
            context.Geocaches.Add(cache);
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                context.Dispose();
            }

            this.disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}