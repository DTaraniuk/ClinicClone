using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Ward
    {
        public Ward()
        {
            Hospitalizations = new HashSet<Hospitalization>();
        }

        public int Id { get; set; }
        public int? Number { get; set; }
        public int? DepartmentId { get; set; }
        public int? Capacity { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
    }
}
