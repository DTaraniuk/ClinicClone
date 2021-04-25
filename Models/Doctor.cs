using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Doctor
    {
        public Doctor()
        {
            Departments = new HashSet<Department>();
            Diagnoses = new HashSet<Diagnosis>();
            DoctorOffices = new HashSet<DoctorOffice>();
            DoctorSpecialities = new HashSet<DoctorSpeciality>();
            Patients = new HashSet<Patient>();
        }

        [Display(Name = "ID лікаря")]
        public int Id { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "ID відділу")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Відділ")]
        public virtual Department Department { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<DoctorOffice> DoctorOffices { get; set; }
        public virtual ICollection<DoctorSpeciality> DoctorSpecialities { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
