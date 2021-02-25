using System;
using System.Collections.Generic;

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

        public int Id { get; set; }
        public string Name { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<DoctorOffice> DoctorOffices { get; set; }
        public virtual ICollection<DoctorSpeciality> DoctorSpecialities { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
