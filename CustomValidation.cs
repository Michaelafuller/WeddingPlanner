using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner
{
    public class FutureDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Because our Date field is optional, can't convert null to date.
            if (value == null)
            {
                return new ValidationResult("is required.");
            }

            DateTime date = (DateTime)value;

            if (date <= DateTime.Now)
            {
                Console.WriteLine("Date was in the past");
                return new ValidationResult("must be in the future.");
            }
            Console.WriteLine("Date was in the future");
            return ValidationResult.Success;
        }
    }
}