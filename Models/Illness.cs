using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
#nullable disable

namespace ClinicWebApplication
{
    public partial class Illness
    {
        public Illness()
        {
            Diagnoses = new HashSet<Diagnosis>();
            IllnessSymptoms = new HashSet<IllnessSymptom>();
        }

        [Display(Name = "ID хвороби")]
        public int Id { get; set; }
        [Display(Name = "Назва хвороби")]
        [Remote(action: "CheckIllnessName", controller: "Illnesses", ErrorMessage = "Така хвороба вже існує")]
        public string Name { get; set; }

        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<IllnessSymptom> IllnessSymptoms { get; set; }
    }

    public struct IllnessWithProbability
    {
        public string Name;
        public double Prob;

        public IllnessWithProbability(string name, double prob)
        {
            Name = name;
            Prob = prob;
        }
    }
}
