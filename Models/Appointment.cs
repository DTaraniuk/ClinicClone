using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Appointment
    {

        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "ID пацієнта")]
        public int? PatientId { get; set; }
        [Display(Name = "ID графіку")]
        public int? ScheduleId { get; set; }
        [Display(Name = "Час")]
        public DateTime Time { get; set; }
        [Display(Name = "Пацієнт")]
        public virtual Patient Patient { get; set; }
        [Display(Name = "Графік")]
        public virtual Schedule Schedule { get; set; }
    }
}
