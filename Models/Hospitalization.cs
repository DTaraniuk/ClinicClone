using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Hospitalization
    {
        [Display(Name = "ID госпіталізація")]
        public int Id { get; set; }
        [Display(Name = "ID пацієнта")]
        public int? PatientId { get; set; }
        [Display(Name = "ID палати")]
        public int? WardId { get; set; }
        [Display(Name = "Час початку")]
        public DateTime? TimeBegin { get; set; }
        [Display(Name = "Час кінця")]
        public DateTime? TimeEnd { get; set; }

        [Display(Name = "Пацієнт")]
        public virtual Patient Patient { get; set; }
        [Display(Name = "Палата")]
        public virtual Ward Ward { get; set; }
    }
}
