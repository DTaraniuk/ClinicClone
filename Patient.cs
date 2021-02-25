using System;
using System.Collections.Generic;

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

        public int Id { get; set; }
        public string Name { get; set; }
        public int? TherapistId { get; set; }

        public virtual Doctor Therapist { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
        public virtual ICollection<PatientSymptom> PatientSymptoms { get; set; }
    }
}
