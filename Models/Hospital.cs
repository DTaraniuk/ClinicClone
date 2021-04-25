using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Hospital
    {
        public Hospital()
        {
            Departments = new HashSet<Department>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID лікарні")]
        public  int Id { get; set; }

        [Display(Name = "Назва лікарні")]
        public string Name { get; set; }
        [Display(Name = "ID міста")]
        public int? CityId { get; set; }
        [Display(Name = "Адреса")]
        public string Address { get; set; }

        [Display(Name = "Місто")]
        public virtual City City { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
