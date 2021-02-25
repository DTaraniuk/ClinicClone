using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Symptom
    {
        public Symptom()
        {
            IllnessSymptoms = new HashSet<IllnessSymptom>();
            PatientSymptoms = new HashSet<PatientSymptom>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<IllnessSymptom> IllnessSymptoms { get; set; }
        public virtual ICollection<PatientSymptom> PatientSymptoms { get; set; }
    }
}
