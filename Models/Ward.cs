using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Ward
    {
        public Ward()
        {
            Hospitalizations = new HashSet<Hospitalization>();
        }

        [Display(Name = "ID палати")]
        public int Id { get; set; }
        [Display(Name = "Номер палати")]
        public int? Number { get; set; }
        [Display(Name = "ID відділу")]
        public int? DepartmentId { get; set; }
        [Display(Name = "Місткість")]
        public int? Capacity { get; set; }

        [Display(Name = "Відділ")]
        public virtual Department Department { get; set; }
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
    }
}
