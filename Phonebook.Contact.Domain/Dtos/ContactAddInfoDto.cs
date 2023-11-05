using System.ComponentModel.DataAnnotations;

namespace Phonebook.Contact.Domain.Dtos
{
    public class ContactAddInfoDto : IValidatableObject
    {
        public required string ContactId { get; set; }
        public int ContactInfoType { get; set; }
        public required string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Value))
            {
                yield return new ValidationResult("Value cannot be empty.", new[] { nameof(Value) });
            }

            if (string.IsNullOrEmpty(ContactId))
            {
                yield return new ValidationResult("ContactId cannot be empty.", new[] { nameof(ContactId) });
            }
        }
    }
}
