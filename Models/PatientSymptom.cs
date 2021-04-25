using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class PatientSymptom
    {
        [Display(Name = "ID пацієнт-симптому")]
        public int Id { get; set; }
        [Display(Name = "ID пацієнта")]
        public int? PatientId { get; set; }
        [Display(Name = "ID симптому")]
        public int? SymptomId { get; set; }
        [Display(Name = "Час")]
        [Remote(action: "DateIsCorrect", controller: "PatientSymptoms", ErrorMessage = "Неправильна дата або час.")]
        public DateTime Time { get; set; }

        [Display(Name = "Пацієнт")]
        public virtual Patient Patient { get; set; }
        [Display(Name = "Симптом")]
        public virtual Symptom Symptom { get; set; }
    }
}
