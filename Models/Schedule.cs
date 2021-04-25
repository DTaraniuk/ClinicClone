using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClinicWebApplication
{
    public partial class Schedule
    {
        public Schedule()
        {
            Appointments = new HashSet<Appointment>();
        }

        [Display(Name = "ID графіку")]
        public int Id { get; set; }
        [Display(Name = "ID доктор-офісу")]
        public int? DoctorOfficeId { get; set; }
        [Display(Name = "Час початку")]
        public DateTime? StartTime { get; set; }
        [Display(Name = "Час кінця")]
        public DateTime? EndTime { get; set; }
        /*[Display(Name = "Дані")]
        public string Data () { return "Доктор: "+DoctorOffice.Doctor.Name + 
                    "Кабінет: " + DoctorOffice.Office.OfficeNumber + 
                    "Час початку " + StartTime + 
                    "Час кінця " + EndTime; }*/
        [Display(Name = "Лікар-кабінет")]
        public virtual DoctorOffice DoctorOffice { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
