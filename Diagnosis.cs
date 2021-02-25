using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Diagnosis
    {
        public int Id { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public int? IllnessId { get; set; }
        public byte[] Time { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Illness Illness { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
