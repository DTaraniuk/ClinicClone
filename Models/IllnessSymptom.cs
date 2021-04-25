using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class IllnessSymptom
    {
        [Display(Name = "ID симптом-хвороби")]
        public int Id { get; set; }
        [Display(Name = "ID хвороби")]
        public int? IllnessId { get; set; }
        [Display(Name = "ID симптома")]
        public int? SymptomId { get; set; }
        [Display(Name = "Частота")]
        [Required]
        [Range(1, 100, ErrorMessage = "Частота хвороби у відсотках має належати проміжку [1, 100]")]
        public float? Frequency { get; set; }
        [Display(Name = "Хвороба")]
        public virtual Illness Illness { get; set; }
        [Display(Name = "Симптом")]
        public virtual Symptom Symptom { get; set; }
    }
}
