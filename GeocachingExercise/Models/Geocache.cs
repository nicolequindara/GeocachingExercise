using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace GeocachingExercise.Models
{
    public class Geocache : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public Coordinate Coordinate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            Regex regex = new Regex(@"[^\s\w\d]");
            if (regex.IsMatch(Name))
            {
                validationResults.Add(new ValidationResult("The field Name can only contain alphanumeric characters or spaces.", new[] { "Name" }));
            }

            if (Coordinate != null)
            {
                validationResults.AddRange(((IValidatableObject)Coordinate).Validate(validationContext).ToList());
            }
            else
            {
                validationResults.Add(new ValidationResult("The field Latitude is required.", new[] { "Latitude" }));
                validationResults.Add(new ValidationResult("The field Longitude is required.", new[] { "Longitude" }));
            }
            
            return validationResults;
        }
    }
}