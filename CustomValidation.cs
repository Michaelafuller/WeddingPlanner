using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner
{
    public class FutureDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return new ValidationResult("is required");
            }

            DateTime date = (DateTime)value;

            if (date <= DateTime.Now)
            {
                return new ValidationResult("must be a future date.");
            }

            return ValidationResult.Success;
        }
    }
}