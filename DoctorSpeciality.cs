using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class DoctorSpeciality
    {
        public int Id { get; set; }
        public int? DoctorId { get; set; }
        public int? SpecialityId { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Speciality Speciality { get; set; }
    }
}
