using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicWebApplication.Models
{
    public class ErrorViewModel
    {
        [Display(Name = "ID запиту")]
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
