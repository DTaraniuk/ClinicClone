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
    public class HospitalizationsController : Controller
    {
        private readonly DBClinicContext _context;

        public HospitalizationsController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: Hospitalizations
        public async Task<IActionResult> Index()
        {
            var dBClinicContext = _context.Hospitalizations.Include(h => h.Patient).Include(h => h.Ward);
            return View(await dBClinicContext.ToListAsync());
        }

        // GET: Hospitalizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalization = await _context.Hospitalizations
                .Include(h => h.Patient)
                .Include(h => h.Ward)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospitalization == null)
            {
                return NotFound();
            }

            return View(hospitalization);
        }

        // GET: Hospitalizations/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name");
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Number");
            return View();
        }

        // POST: Hospitalizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,WardId,TimeBegin,TimeEnd")] Hospitalization hospitalization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospitalization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", hospitalization.PatientId);
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Number", hospitalization.WardId);
            return View(hospitalization);
        }

        // GET: Hospitalizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalization = await _context.Hospitalizations.FindAsync(id);
            if (hospitalization == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", hospitalization.PatientId);
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Number", hospitalization.WardId);
            return View(hospitalization);
        }

        // POST: Hospitalizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,WardId,TimeBegin,TimeEnd")] Hospitalization hospitalization)
        {
            if (id != hospitalization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospitalization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalizationExists(hospitalization.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", hospitalization.PatientId);
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Number", hospitalization.WardId);
            return View(hospitalization);
        }

        // GET: Hospitalizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalization = await _context.Hospitalizations
                .Include(h => h.Patient)
                .Include(h => h.Ward)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospitalization == null)
            {
                return NotFound();
            }

            return View(hospitalization);
        }

        // POST: Hospitalizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospitalization = await _context.Hospitalizations.FindAsync(id);
            _context.Hospitalizations.Remove(hospitalization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalizationExists(int id)
        {
            return _context.Hospitalizations.Any(e => e.Id == id);
        }
    }
}
