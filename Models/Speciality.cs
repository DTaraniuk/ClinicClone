using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Speciality
    {
        public Speciality()
        {
            DoctorSpecialities = new HashSet<DoctorSpeciality>();
        }

        [Display(Name = "ID спеціальності")]
        public int Id { get; set; }
        [Display(Name = "Назва спеціальності")]
        public string Name { get; set; }

        public virtual ICollection<DoctorSpeciality> DoctorSpecialities { get; set; }
    }
}
