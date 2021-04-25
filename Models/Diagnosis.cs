using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Diagnosis
    {
        [Display(Name = "ID діагнозу")]
        public int Id { get; set; }
        [Display(Name = "ID лікаря")]
        public int? DoctorId { get; set; }
        [Display(Name = "ID пацієнта")]
        public int? PatientId { get; set; }
        [Display(Name = "ID хвороби")]
        public int? IllnessId { get; set; }
        [Display(Name = "Час")]
        public DateTime Time { get; set; }
        [Display(Name = "Лікар")]
        public virtual Doctor Doctor { get; set; }
        [Display(Name = "Хвороба")]
        public virtual Illness Illness { get; set; }
        [Display(Name = "Пацієнт")]
        public virtual Patient Patient { get; set; }
    }
}
