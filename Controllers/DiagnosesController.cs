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
    public class DiagnosesController : Controller
    {
        private readonly DBClinicContext _context;

        public DiagnosesController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: Diagnoses
        public async Task<IActionResult> Index()
        {
            var dBClinicContext = _context.Diagnoses.Include(d => d.Doctor).Include(d => d.Illness).Include(d => d.Patient);
            return View(await dBClinicContext.ToListAsync());
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Doctor)
                .Include(d => d.Illness)
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // GET: Diagnoses/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name");
            ViewData["IllnessId"] = new SelectList(_context.Illnesses, "Id", "Name");
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name");
            return View();
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,PatientId,IllnessId,Time")] Diagnosis diagnosis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", diagnosis.DoctorId);
            ViewData["IllnessId"] = new SelectList(_context.Illnesses, "Id", "Name", diagnosis.IllnessId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", diagnosis.PatientId);
            return View(diagnosis);
        }

        // GET: Diagnoses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", diagnosis.DoctorId);
            ViewData["IllnessId"] = new SelectList(_context.Illnesses, "Id", "Name", diagnosis.IllnessId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", diagnosis.PatientId);
            return View(diagnosis);
        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,PatientId,IllnessId,Time")] Diagnosis diagnosis)
        {
            if (id != diagnosis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisExists(diagnosis.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", diagnosis.DoctorId);
            ViewData["IllnessId"] = new SelectList(_context.Illnesses, "Id", "Name", diagnosis.IllnessId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", diagnosis.PatientId);
            return View(diagnosis);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Doctor)
                .Include(d => d.Illness)
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosisExists(int id)
        {
            return _context.Diagnoses.Any(e => e.Id == id);
        }
    }
}
