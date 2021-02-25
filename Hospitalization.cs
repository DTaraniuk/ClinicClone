using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Hospitalization
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? WardId { get; set; }
        public TimeSpan? TimeBegin { get; set; }
        public TimeSpan? TimeEnd { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Ward Ward { get; set; }
    }
}
