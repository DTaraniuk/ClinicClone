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
    public class IllnessesSymptomsChartController : ControllerBase
    {
        public readonly DBClinicContext _context;
        public IllnessesSymptomsChartController(DBClinicContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var illnesses = _context.Illnesses.ToList();
            List<object> departmentsDoctors = new List<object>();
            departmentsDoctors.Add(new object[] { "Хвороба", "Кількість симтомів" });
            foreach (var i in illnesses)
            {
                Console.WriteLine(i.Name);
                departmentsDoctors.Add(new object[] { i.Name, _context.IllnessSymptoms.Count(D => D.IllnessId == i.Id) });
            }
            return new JsonResult(departmentsDoctors);
        }
    }
}
