using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsDoctorsChartController : ControllerBase
    {
        public readonly DBClinicContext _context;
        public DepartmentsDoctorsChartController(DBClinicContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var departments = _context.Departments.ToList();
            List<object> departmentsDoctors = new List<object>();
            departmentsDoctors.Add(new object[] { "Віділення", "Кількість лікарів" });
            foreach (var d in departments)
            {
                Console.WriteLine(d.Name);
                departmentsDoctors.Add(new object[] { d.Name,  _context.Doctors.Count(D=>D.DepartmentId==d.Id)});
            }
            return new JsonResult(departmentsDoctors);
        }
    }
}
