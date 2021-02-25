using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Illness
    {
        public Illness()
        {
            Diagnoses = new HashSet<Diagnosis>();
            IllnessSymptoms = new HashSet<IllnessSymptom>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<IllnessSymptom> IllnessSymptoms { get; set; }
    }
}
