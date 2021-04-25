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
    public class DoctorSpecialitiesController : Controller
    {
        private readonly DBClinicContext _context;

        public DoctorSpecialitiesController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: DoctorSpecialities
        public async Task<IActionResult> Index(int? dsid, string? name, string? from)
        {
            if(from == null)
            {
                var dBClinicContext = _context.DoctorSpecialities.Include(d => d.Doctor).Include(d => d.Speciality);
                return View(await dBClinicContext.ToListAsync());
            }
            else if(from == "doc")
            {
                ViewBag.From = from;
                ViewBag.Id = dsid;
                ViewBag.Name = name;
                var dBClinicContext = _context.DoctorSpecialities.Where(DS => DS.DoctorId==dsid).Include(d => d.Doctor).Include(d => d.Speciality);
                return View(await dBClinicContext.ToListAsync());
            }
            else
            {
                ViewBag.From = from;
                ViewBag.Id = dsid;
                ViewBag.Name = name;
                var dBClinicContext = _context.DoctorSpecialities.Where(DS => DS.SpecialityId==dsid).Include(d => d.Doctor).Include(d => d.Speciality);
                return View(await dBClinicContext.ToListAsync());
            }
        }

        // GET: DoctorSpecialities/Details/5
        /*public async Task<IActionResult> Details(int? itemId, int? id, string? name, string? from)
        {
            if (itemId == null)
            {
                return NotFound();
            }

            var doctorSpeciality = await _context.DoctorSpecialities
                .Include(d => d.Doctor)
                .Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.Id == itemId);
            if (doctorSpeciality == null)
            {
                return NotFound();
            }

            ViewBag.From = from;
            ViewBag.Id = id;
            ViewBag.Name = name;

            return View(doctorSpeciality);
        }*/

        // GET: DoctorSpecialities/Create
        public IActionResult Create(int? dsid, string? name, string? from)
        {
            ViewBag.From = from;
            ViewBag.Id = dsid;
            ViewBag.Name = name;
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name");
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name");
            return View();
            
        }

        // POST: DoctorSpecialities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,SpecialityId")] DoctorSpeciality doctorSpeciality)
        {
            doctorSpeciality.Id = 0;
            if (ModelState.IsValid)
            {
                _context.Add(doctorSpeciality);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", doctorSpeciality.DoctorId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", doctorSpeciality.SpecialityId);
            return View(doctorSpeciality);
        }

        public async Task<IActionResult> CreateForDoc([Bind("Id,DoctorId,SpecialityId")] DoctorSpeciality doctorSpeciality, int? dsid, string? name)
        {
            doctorSpeciality.Id = 0;
            if (ModelState.IsValid)
            {
                _context.Add(doctorSpeciality);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "DoctorSpecialities", new { dsid = dsid, name = name, from="doc"});
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", doctorSpeciality.DoctorId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", doctorSpeciality.SpecialityId);
            return View(doctorSpeciality);
        }

        public async Task<IActionResult> CreateForSpec([Bind("Id,DoctorId,SpecialityId")] DoctorSpeciality doctorSpeciality, int? dsid, string? name)
        {
            doctorSpeciality.Id = 0;
            if (ModelState.IsValid)
            {
                _context.Add(doctorSpeciality);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "DoctorSpecialities", new { dsid = dsid, name = name, from="spec"});
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", doctorSpeciality.DoctorId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", doctorSpeciality.SpecialityId);
            return View(doctorSpeciality);
        }

        // GET: DoctorSpecialities/Edit/5
        public async Task<IActionResult> Edit(int? id, int? dsid, string? name, string? from)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorSpeciality = await _context.DoctorSpecialities.FindAsync(id);
            if (doctorSpeciality == null)
            {
                return NotFound();
            }
            ViewBag.From = from;
            ViewBag.Id = dsid;
            ViewBag.Name = name;
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", doctorSpeciality.DoctorId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", doctorSpeciality.SpecialityId);
            return View(doctorSpeciality);
        }

        // POST: DoctorSpecialities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,SpecialityId")] DoctorSpeciality doctorSpeciality, int? dsid, string? name, string? from)
        {
            if (id != doctorSpeciality.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorSpeciality);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorSpecialityExists(doctorSpeciality.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if(from == "doc")
                {
                    return RedirectToAction("Index", "DoctorSpecialities", new { dsid = dsid, name = name, from = from });
                }
                else if(from == "spec")
                {
                    return RedirectToAction("Index", "DoctorSpecialities", new { dsid = dsid, name = name, from = from });
                }
                else return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", doctorSpeciality.DoctorId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Name", doctorSpeciality.SpecialityId);
            return View(doctorSpeciality);
        }

        // GET: DoctorSpecialities/Delete/5
        public async Task<IActionResult> Delete(int? id, int? dsid, string? name, string? from)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorSpeciality = await _context.DoctorSpecialities
                .Include(d => d.Doctor)
                .Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctorSpeciality == null)
            {
                return NotFound();
            }
            ViewBag.From = from;
            ViewBag.Id = dsid;
            ViewBag.Name = name;

            return View(doctorSpeciality);
        }

        // POST: DoctorSpecialities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? dsid, string? name, string? from)
        {
            var doctorSpeciality = await _context.DoctorSpecialities.FindAsync(id);
            _context.DoctorSpecialities.Remove(doctorSpeciality);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "DoctorSpecialities", new { dsid = dsid, name = name, from = from }); 
        }

        private bool DoctorSpecialityExists(int id)
        {
            return _context.DoctorSpecialities.Any(e => e.Id == id);
        }
    }
}
