using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class DoctorSpeciality
    {
        [Display(Name = "ID спеціальності лікаря")]
        public int Id { get; set; }
        [Display(Name = "ID лікаря")]
        public int? DoctorId { get; set; }
        [Display(Name = "ID спеціальності")]
        public int? SpecialityId { get; set; }

        [Display(Name = "Лікар")]
        public virtual Doctor Doctor { get; set; }
        [Display(Name = "Спеціальність")]
        public virtual Speciality Speciality { get; set; }
    }
}
