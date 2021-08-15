using System;
using System.Collections.Generic;

#nullable disable

namespace Vensa.Api.Entities
{
    public partial class Consumer : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
    }
}
