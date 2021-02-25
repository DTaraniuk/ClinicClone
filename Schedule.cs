using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Schedule
    {
        public Schedule()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public int? DoctorOfficeId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual DoctorOffice DoctorOffice { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
