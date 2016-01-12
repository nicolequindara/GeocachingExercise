using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace GeocachingExercise.Models
{
    public class Coordinate : IValidatableObject
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinate()
        {
        }
        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            
            if(Math.Abs(Latitude) > 90.0)
            {
                yield return new ValidationResult("The field Latitude must range from -90.0 to 90.0.", new List<String> { "Latitude" });
            }

            if (Math.Abs(Longitude) > 180.0)
            {
                yield return new ValidationResult("The field Longitude must range from -180.0 to 180.0.", new List<String> { "Longitude" });
            }            
        }
    }
}