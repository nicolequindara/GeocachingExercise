using System;
using System.Collections.Generic;

using GeocachingExercise.Models;

namespace GeocachingExercise.Persistence.EF
{
    public interface IGeocacheRepository : IDisposable
    {
        IEnumerable<Geocache> All();
        Geocache Find(int? Id);

        void Create(Geocache cache);
        void Save();
    }
}