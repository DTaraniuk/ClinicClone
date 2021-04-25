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
    public class PatientSymptomsController : Controller
    {
        private readonly DBClinicContext _context;

        public PatientSymptomsController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: PatientSymptoms
        public async Task<IActionResult> Index(int? patientId)
        {
            ViewBag.patientId = patientId;
            ViewBag.patientName = _context.Patients.Where(p => p.Id == patientId).FirstOrDefault().Name;
            var dBClinicContext = _context.PatientSymptoms.Where(ps => ps.PatientId == patientId).Include(ps => ps.Symptom);
            return View(await dBClinicContext.ToListAsync());
        }

        public async Task<IActionResult> Determine(int? patientId)
        {
            return RedirectToAction("IndexByPatient", "Illnesses", new { id = patientId });
        }

        // GET: PatientSymptoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientSymptom = await _context.PatientSymptoms
                .Include(p => p.Patient)
                .Include(p => p.Symptom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientSymptom == null)
            {
                return NotFound();
            }

            return View(patientSymptom);
        }

        // GET: PatientSymptoms/Create
        public IActionResult Create(int? patientId)
        {
            ViewBag.patientId = patientId;
            ViewBag.patientName = _context.Patients.Where(p => p.Id == patientId).FirstOrDefault().Name;
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Name");
            return View();
        }

        // POST: PatientSymptoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,SymptomId,Time")] PatientSymptom patientSymptom, int? patientId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientSymptom);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "PatientSymptoms", new { patientId = patientId });
            }
            ViewBag.patientId = patientId;
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Name", patientSymptom.SymptomId);
            return View(patientSymptom);
        }

        // GET: PatientSymptoms/Edit/5
        public async Task<IActionResult> Edit(int? id, int? patientId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientSymptom = await _context.PatientSymptoms.FindAsync(id);
            if (patientSymptom == null)
            {
                return NotFound();
            }

            ViewBag.patientId = patientId;
            ViewBag.patientName = _context.Patients.Where(p => p.Id == patientId).FirstOrDefault().Name;
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Name", patientSymptom.SymptomId);
            return View(patientSymptom);
        }

        // POST: PatientSymptoms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,SymptomId,Time")] PatientSymptom patientSymptom, int? patientId)
        {
            if (id != patientSymptom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientSymptom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientSymptomExists(patientSymptom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "PatientSymptoms", new { patientId = patientId });
            }

            ViewBag.patientId = patientId;
            ViewBag.patientName = _context.Patients.Where(p => p.Id == patientId).FirstOrDefault().Name;
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Name", patientSymptom.SymptomId);
            return View(patientSymptom);
        }

        // GET: PatientSymptoms/Delete/5
        public async Task<IActionResult> Delete(int? id, int? patientId)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.patientId = patientId;
            ViewBag.patientName = _context.Patients.Where(p => p.Id == patientId).FirstOrDefault().Name;
            var patientSymptom = await _context.PatientSymptoms
                .Include(p => p.Patient)
                .Include(p => p.Symptom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientSymptom == null)
            {
                return NotFound();
            }

            return View(patientSymptom);
        }

        // POST: PatientSymptoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? patientId)
        {
            ViewBag.patientId = patientId;
            ViewBag.patientName = _context.Patients.Where(p => p.Id == patientId).FirstOrDefault().Name;
            var patientSymptom = await _context.PatientSymptoms.FindAsync(id);
            _context.PatientSymptoms.Remove(patientSymptom);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "PatientSymptoms", new { patientId = patientId });
        }

        private bool PatientSymptomExists(int id)
        {
            return _context.PatientSymptoms.Any(e => e.Id == id);
        }

        public bool DateIsCorrect(DateTime Time)
        {
            return Time < DateTime.Now;
        }
    }
}
