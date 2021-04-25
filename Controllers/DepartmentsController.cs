using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication;

namespace ClinicWebApplication.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly DBClinicContext _context;

        public DepartmentsController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                var dBClinicContext = _context.Departments.Include(d => d.DepartmentHead).Include(d => d.Hospital);
                return View(await dBClinicContext.ToListAsync());
            }
            var deps_by_hosp = _context.Departments.Where(d => d.HospitalId == id).Include(d => d.DepartmentHead).Include(d => d.Hospital);
            ViewBag.hospitalId = id;
            ViewBag.hospitalName = _context.Hospitals.Where(x => x.Id == id).FirstOrDefault().Name;
            ViewBag.cityId = _context.Hospitals.Where(x=>x.Id==id).FirstOrDefault().CityId;
            return View(await deps_by_hosp.ToListAsync());

        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id, int? hospitalId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.DepartmentHead)
                .Include(d => d.Hospital)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            //return View(department);
            return RedirectToAction("Index", "Doctors", new {id = department.Id, name = department.Name,
                from = "dep", hospitalId=hospitalId});
        }

        // GET: Departments/Create
        public IActionResult Create(int? hospitalId)
        {
            ViewData["DepartmentHeadId"] = new SelectList(_context.Doctors, "Id", "Name");
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name");
            ViewBag.hospitalIdx = hospitalId;
            ViewBag.hospitalNamex = _context.Hospitals.Where(x => x.Id == hospitalId).Select(x => x.Name).FirstOrDefault();
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,HospitalId,DepartmentHeadId")] Department department, int? hospitalId)
        {
            department.Id = 0;
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id = hospitalId});
            }
            ViewData["DepartmentHeadId"] = new SelectList(_context.Doctors, "Id", "Name", department.DepartmentHeadId);
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Name", department.HospitalId);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id, int? hospitalId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["DepartmentHeadId"] = new SelectList(_context.Doctors, "Id", "Id", department.DepartmentHeadId);
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id", department.HospitalId);
            ViewBag.hospitalId = hospitalId;
            ViewBag.hospitalName = _context.Hospitals.Where(x => x.Id == hospitalId).FirstOrDefault().Name;
            ViewBag.departmentName = _context.Departments.Where(x => x.Id == id).FirstOrDefault().Name;
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,HospitalId,DepartmentHeadId")] Department department, int? departmentId)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.departmentId = departmentId;
            ViewData["DepartmentHeadId"] = new SelectList(_context.Doctors, "Id", "Id", department.DepartmentHeadId);
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id", department.HospitalId);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id, int? hospitalId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.DepartmentHead)
                .Include(d => d.Hospital)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            ViewBag.hospitalId = hospitalId;
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctors = _context.Doctors.Where(d => d.DepartmentId == id);
            foreach (Doctor d in doctors)
            {
                int doctorId = d.Id;
                var doctorOffices = _context.DoctorOffices.Where(DO => DO.DoctorId == doctorId);
                _context.DoctorOffices.RemoveRange(doctorOffices);
                var diagnoses = _context.Diagnoses.Where(d => d.DoctorId == doctorId);
                _context.Diagnoses.RemoveRange(diagnoses);
                var patients = _context.Patients.Where(p => p.TherapistId == doctorId);
                _context.Patients.RemoveRange(patients);
                var departments = _context.Departments.Where(d => d.DepartmentHeadId == doctorId);
                _context.Departments.RemoveRange(departments);

                var doctor = await _context.Doctors.FindAsync(doctorId);
                _context.Doctors.Remove(doctor);
            }

            var wards = _context.Wards.Where(w => w.DepartmentId == id);
            _context.Wards.RemoveRange(wards);

            var department = await _context.Departments.FindAsync(id);
            var hid = department.HospitalId;
            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Departments", new { id = hid });
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
