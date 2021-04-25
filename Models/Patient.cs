using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Patient
    {
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
            Diagnoses = new HashSet<Diagnosis>();
            Hospitalizations = new HashSet<Hospitalization>();
            PatientSymptoms = new HashSet<PatientSymptom>();
        }

        [Display(Name = "ID пацієнта")]
        public int Id { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "ID терапіста")]
        public int? TherapistId { get; set; }

        [Display(Name = "Терапевт")]
        public virtual Doctor Therapist { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
        public virtual ICollection<PatientSymptom> PatientSymptoms { get; set; }
    }
}
