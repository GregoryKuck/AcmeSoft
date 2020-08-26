using System;
using System.Collections.Generic;

namespace AcmeSoft.Data.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Employee> Employees { get; set; }

    }
}
