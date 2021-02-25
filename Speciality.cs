using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Speciality
    {
        public Speciality()
        {
            DoctorSpecialities = new HashSet<DoctorSpeciality>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DoctorSpeciality> DoctorSpecialities { get; set; }
    }
}
