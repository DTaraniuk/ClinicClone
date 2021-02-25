using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Appointment
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? ScheduleId { get; set; }
        public byte[] Time { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}
