using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Department
    {
        public Department()
        {
            Doctors = new HashSet<Doctor>();
            Wards = new HashSet<Ward>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? HospitalId { get; set; }
        public int? DepartmentHeadId { get; set; }

        public virtual Doctor DepartmentHead { get; set; }
        public virtual Hospital Hospital { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
