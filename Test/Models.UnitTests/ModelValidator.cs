using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public abstract class ModelValidator
    {
        public List<ValidationResult> ValidateModel(object model)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            ValidationContext context = new ValidationContext(model, null, null);

            Validator.TryValidateObject(model, context, validationResults, true);

            return validationResults;
        }
    }
}
