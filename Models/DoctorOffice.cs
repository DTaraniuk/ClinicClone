using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class DoctorOffice
    {
        public DoctorOffice()
        {
            Schedules = new HashSet<Schedule>();
        }

        [Display(Name = "ID кабінету лікаря")]
        public int Id { get; set; }
        [Display(Name = "ID доктора")]
        public int? DoctorId { get; set; }
        [Display(Name = "ID кабінету")]
        public int? OfficeId { get; set; }

        [Display(Name = "Лікар")]
        public virtual Doctor Doctor { get; set; }
        [Display(Name = "Кабінет")]
        public virtual Office Office { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
