using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class DoctorOffice
    {
        public DoctorOffice()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public int? DoctorId { get; set; }
        public int? OfficeId { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Office Office { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
