using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Contact.Domain.VOs
{
    public class ContactDetailVO : IValidatableObject
    {
        public required string Id { get; set; }
        public required string ContactId { get; set; }
        public int ContactInfoType { get; set; }
        public required string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Value))
            {
                yield return new ValidationResult("Value boş olamaz.", new[] { nameof(Value) });
            }

            if (string.IsNullOrEmpty(ContactId))
            {
                yield return new ValidationResult("ContactId boş olamaz.", new[] { nameof(ContactId) });
            }
        }
    }
}
