﻿using System.ComponentModel.DataAnnotations;

namespace Phonebook.Contact.Domain.Dtos
{
    public class ContactUpdateDto : IValidatableObject
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Company { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Id))
            {
                yield return new ValidationResult("field cannot be empty", new[] { nameof(Id) });
            }
            else if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("field cannot be empty", new[] { nameof(Name) });
            }
            else if (string.IsNullOrEmpty(LastName))
            {
                yield return new ValidationResult("field cannot be empty.", new[] { nameof(LastName) });
            }
            else if (string.IsNullOrEmpty(Company))
            {
                yield return new ValidationResult("field cannot be empty", new[] { nameof(Company) });
            }
        }
    }
}
