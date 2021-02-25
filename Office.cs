using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Office
    {
        public Office()
        {
            DoctorOffices = new HashSet<DoctorOffice>();
        }

        public int Id { get; set; }
        public int? OfficeNumber { get; set; }

        public virtual ICollection<DoctorOffice> DoctorOffices { get; set; }
    }
}
