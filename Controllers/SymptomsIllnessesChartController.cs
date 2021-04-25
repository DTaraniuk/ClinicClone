using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomsIllnessesChartController : ControllerBase
    {
        public readonly DBClinicContext _context;
        public SymptomsIllnessesChartController(DBClinicContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var symptoms = _context.Symptoms.ToList();
            List<object> departmentsDoctors = new List<object>();
            departmentsDoctors.Add(new object[] { "Симптом", "Кількість хвороб" });
            foreach (var s in symptoms)
            {
                Console.WriteLine(s.Name);
                departmentsDoctors.Add(new object[] { s.Name, _context.IllnessSymptoms.Count(D => D.SymptomId == s.Id) });
            }
            return new JsonResult(departmentsDoctors);
        }
    }
}
