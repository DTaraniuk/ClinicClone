using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Office
    {
        public Office()
        {
            DoctorOffices = new HashSet<DoctorOffice>();
        }

        [Display(Name = "ID кабінету")]
        public int Id { get; set; }
        [Display(Name = "Номер кабінету")]
        public int? OfficeNumber { get; set; }

        public virtual ICollection<DoctorOffice> DoctorOffices { get; set; }
    }
}
