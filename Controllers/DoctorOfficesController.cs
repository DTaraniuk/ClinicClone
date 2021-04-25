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
    public class DoctorOfficesController : Controller
    {
        private readonly DBClinicContext _context;

        public DoctorOfficesController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: DoctorOffices
        public async Task<IActionResult> Index()
        {
            var dBClinicContext = _context.DoctorOffices.Include(d => d.Doctor).Include(d => d.Office);
            return View(await dBClinicContext.ToListAsync());
        }

        // GET: DoctorOffices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorOffice = await _context.DoctorOffices
                .Include(d => d.Doctor)
                .Include(d => d.Office)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctorOffice == null)
            {
                return NotFound();
            }

            return View(doctorOffice);
        }

        // GET: DoctorOffices/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name");
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "OfficeNumber");
            return View();
        }

        // POST: DoctorOffices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,OfficeId")] DoctorOffice doctorOffice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctorOffice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", doctorOffice.DoctorId);
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "OfficeNumber", doctorOffice.OfficeId);
            return View(doctorOffice);
        }

        // GET: DoctorOffices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorOffice = await _context.DoctorOffices.FindAsync(id);
            if (doctorOffice == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", doctorOffice.DoctorId);
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "OfficeNumber", doctorOffice.OfficeId);
            return View(doctorOffice);
        }

        // POST: DoctorOffices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,OfficeId")] DoctorOffice doctorOffice)
        {
            if (id != doctorOffice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorOffice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorOfficeExists(doctorOffice.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Name", doctorOffice.DoctorId);
            ViewData["OfficeId"] = new SelectList(_context.Offices, "Id", "OfficeNumber", doctorOffice.OfficeId);
            return View(doctorOffice);
        }

        // GET: DoctorOffices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorOffice = await _context.DoctorOffices
                .Include(d => d.Doctor)
                .Include(d => d.Office)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctorOffice == null)
            {
                return NotFound();
            }

            return View(doctorOffice);
        }

        // POST: DoctorOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedules = _context.Schedules.Where(s => s.DoctorOfficeId == id);
            _context.Schedules.RemoveRange(schedules);

            var doctorOffice = await _context.DoctorOffices.FindAsync(id);
            _context.DoctorOffices.Remove(doctorOffice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorOfficeExists(int id)
        {
            return _context.DoctorOffices.Any(e => e.Id == id);
        }
    }
}
