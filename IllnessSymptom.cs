using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class IllnessSymptom
    {
        public int Id { get; set; }
        public int? IllnessId { get; set; }
        public int? SymptomId { get; set; }
        public float? Frequency { get; set; }

        public virtual Illness Illness { get; set; }
        public virtual Symptom Symptom { get; set; }
    }
}
