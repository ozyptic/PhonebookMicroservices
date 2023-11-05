﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Contact.Domain.VOs
{
    public class ContactVO : IValidatableObject
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Company { get; set; }
        public IList<ContactDetailVO> ContactDetailVOs { get; set; } = new List<ContactDetailVO>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("boş olamaz.", new[] { nameof(Name) });
            }
            else if (string.IsNullOrEmpty(LastName))
            {
                yield return new ValidationResult("boş olamaz.", new[] { nameof(LastName) });
            }
            else if (string.IsNullOrEmpty(Company))
            {
                yield return new ValidationResult("boş olamaz", new[] { nameof(Company) });
            }
        }
    }
}
