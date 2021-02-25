using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class City
    {
        public City()
        {
            Hospitals = new HashSet<Hospital>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Hospital> Hospitals { get; set; }
    }
}
