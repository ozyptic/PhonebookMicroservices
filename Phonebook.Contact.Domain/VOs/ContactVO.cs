using System;
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
        public IList<ContactInfoVO> ContactInfoVOs { get; set; } = new List<ContactInfoVO>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("field cannot be empty", new[] { nameof(Name) });
            }
            else if (string.IsNullOrEmpty(LastName))
            {
                yield return new ValidationResult("field cannot be empty", new[] { nameof(LastName) });
            }
            else if (string.IsNullOrEmpty(Company))
            {
                yield return new ValidationResult("field cannot be empty", new[] { nameof(Company) });
            }
        }
    }
}
