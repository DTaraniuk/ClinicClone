using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class City
    {
        public City()
        {
            Hospitals = new HashSet<Hospital>();
        }

        [Display(Name = "ID міста")]
        public int Id { get; set; }
        [Display(Name = "Назва міста")]
        public string Name { get; set; }

        public virtual ICollection<Hospital> Hospitals { get; set; }
    }
}
