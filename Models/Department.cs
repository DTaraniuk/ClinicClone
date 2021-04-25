using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "ID відділу")]
        public int Id { get; set; }
        [Display(Name = "Назва відділення")]
        public string Name { get; set; }
        [Display(Name = "ID лікарні")]
        public int? HospitalId { get; set; }
        [Display(Name = "ID голови департименту")]
        public int? DepartmentHeadId { get; set; }

        [Display(Name = "Голова відділу")]
        public virtual Doctor DepartmentHead { get; set; }
        [Display(Name = "Лікарня")]
        public virtual Hospital Hospital { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
