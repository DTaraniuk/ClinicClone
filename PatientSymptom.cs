using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class PatientSymptom
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? SymptomId { get; set; }
        public byte[] Time { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Symptom Symptom { get; set; }
    }
}
