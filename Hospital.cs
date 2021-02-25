using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Hospital
    {
        public Hospital()
        {
            Departments = new HashSet<Department>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
