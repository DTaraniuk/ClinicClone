using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Symptom
    {
        public Symptom()
        {
            IllnessSymptoms = new HashSet<IllnessSymptom>();
            PatientSymptoms = new HashSet<PatientSymptom>();
        }

        [Display(Name = "ID симптому")]
        public int Id { get; set; }
        [Display(Name = "Назва симптому")]
        [Remote(action: "CheckSymptomName", controller: "Symptoms", ErrorMessage = "Такий симптом вже існує")]
        public string Name { get; set; }

        public virtual ICollection<IllnessSymptom> IllnessSymptoms { get; set; }
        public virtual ICollection<PatientSymptom> PatientSymptoms { get; set; }
    }
}
