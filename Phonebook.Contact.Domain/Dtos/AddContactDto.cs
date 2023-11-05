using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Contact.Domain.Dtos
{
    public class AddContact
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Company { get; set; }
    }
}
