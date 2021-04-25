using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ClinicWebApplication.Controllers
{
    public class IllnessesController : Controller
    {
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckIllnessName(string name)
        {
            foreach(var i in _context.Illnesses.ToList())
                if (Equalizer.CompareStrings(i.Name, name))
                return Json(false);
            return Json(true);
        }

        private readonly DBClinicContext _context;

        public IllnessesController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: Illnesses
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (name == null)
            {
               return View(await _context.Illnesses.ToListAsync());
            }

            ViewBag.SymptomId = id;
            ViewBag.SymptomName = name;
            var illnessesBySymptom = _context.IllnessSymptoms.Where(IS =>IS.SymptomId == id).Select(IS => IS.Illness);
            return View(await illnessesBySymptom.ToListAsync());
        }

        public async Task<IActionResult> IndexByPatient(int id)
        {
            ViewBag.PatientId = id;
            ViewBag.PatientName = _context.Patients.Where(p => p.Id == id).FirstOrDefault().Name;

            var symptoms = _context.PatientSymptoms.Where(ps => ps.PatientId == id).Select(ps => new { id = ps.SymptomId, time = ps.Time } );

            List<IllnessWithProbability> illnesses = new List<IllnessWithProbability>();

            double totalProb = 0.0;
            foreach(Illness i in _context.Illnesses)
            {
                double prob = 1.0;
                foreach (var s in symptoms)
                {
                    double freq = _context.IllnessSymptoms.Any(IS => IS.IllnessId == i.Id && IS.SymptomId == s.id) ?
                        ((double)_context.IllnessSymptoms.Where(IS => IS.IllnessId == i.Id && IS.SymptomId == s.id).FirstOrDefault().Frequency)/100.0 :
                        1.0 - 1.0 / ( ((DateTime.Now - s.time).Days + 1.0) / 10.0 + 1.0 );
                    prob *= freq;
                }

                foreach(var IS in _context.IllnessSymptoms.Where(IS => IS.IllnessId == i.Id && !symptoms.Any(s => s.id == IS.SymptomId)))
                {
                    double freq = 1.0 - ((double)IS.Frequency) / 100.0;
                    prob *= freq;
                }
                illnesses.Add(new IllnessWithProbability(i.Name, prob));
                totalProb += prob;
            }
            if (totalProb == 0.0)
                totalProb = 1.0;

            var sortedList = illnesses.Select(i => new IllnessWithProbability(i.Name, i.Prob * 100.0 / totalProb)).OrderByDescending(i => i.Prob);
            return View(sortedList.ToList());
        }

        // GET: Illnesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var illness = await _context.Illnesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (illness == null)
            {
                return NotFound();
            }

            return RedirectToAction("IndexByIllness", "IllnessSymptoms", new { id = illness.Id, name = illness.Name });
        }

        // GET: Illnesses/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateIS(int symptomId)
        {
            return RedirectToAction("Create", "IllnessSymptoms", new { illnessId = -1, symptomId = symptomId });
        }


        // POST: Illnesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Illness illness)
        {
            if (ModelState.IsValid)
            {
                _context.Add(illness);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(illness);
        }

        // GET: Illnesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var illness = await _context.Illnesses.FindAsync(id);
            if (illness == null)
            {
                return NotFound();
            }
            return View(illness);
        }
        public async Task<IActionResult> EditIS(int? id, int? symptomId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var illness = await _context.Illnesses.FindAsync(id);
            if (illness == null)
            {
                return NotFound();
            }
            return RedirectToAction("Edit", "IllnessSymptoms", new { illnessId = id, symptomId = symptomId });
            return View(illness);
        }
        // POST: Illnesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // POST: Illnesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Illness illness)
        {
            if (id != illness.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(illness);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IllnessExists(illness.Id))
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
            return View(illness);
        }
        // GET: Illnesses/Delete/5
        public async Task<IActionResult> Delete(int? id, int? symptomId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var illness = await _context.Illnesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (illness == null)
            {
                return NotFound();
            }

            return View(illness);
        }

        public async Task<IActionResult> DeleteIS(int? id, int? symptomId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var illness = await _context.Illnesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (illness == null)
            {
                return NotFound();
            }

            return RedirectToAction("Delete", "IllnessSymptoms", new { illnessId = id, symptomId = symptomId });
            return View(illness);
        }

        // POST: Illnesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var illnessSymptoms = _context.IllnessSymptoms.Where(IS => IS.IllnessId == id);
            _context.IllnessSymptoms.RemoveRange(illnessSymptoms);

            var illness = await _context.Illnesses.FindAsync(id);
            _context.Illnesses.Remove(illness);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IllnessExists(int id)
        {
            return _context.Illnesses.Any(e => e.Id == id);
        }


        async Task addSymptom(String s)
        {
            int idS = -1;
            try
            {
                idS = _context.Symptoms.Where(S => S.Name.Equals(s)).FirstOrDefault().Id;
            }
            catch (Exception e)
            {
                Symptom sym = new Symptom();
                sym.Name = s;
                _context.Symptoms.Add(sym);
                await _context.SaveChangesAsync();
            }
        }

        async Task addIllness(String s)
        {
            int idI = -1;
            try
            {
                idI = _context.Illnesses.Where(I => I.Name.Equals(s)).FirstOrDefault().Id;
            }
            catch (Exception e)
            {
                Illness il = new Illness();
                il.Name = s;
                _context.Illnesses.Add(il);
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {

                                await Task.Run(() => (addIllness(worksheet.Name)));
                                Dictionary<String, int> map = new Dictionary<String, int>();

                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    for (int i = 1; i <= 10; ++i)
                                    {
                                        if (worksheet.Cell(1, i) == null) continue;
                                        map[worksheet.Cell(1, i).Value.ToString().ToLower()] = i;
                                    }
                                    if (!map.ContainsKey("частота") || !map.ContainsKey("симптом"))
                                    {
                                        continue;
                                    }
                                    try
                                    {
                                        IllnessSymptom newIS = new IllnessSymptom();
                                        await Task.Run(() => (addSymptom(row.Cell(map["симптом"]).Value.ToString())));

                                        int idI = _context.Illnesses.Where(I => I.Name.Equals(worksheet.Name)).FirstOrDefault().Id;
                                        newIS.Illness = _context.Illnesses.Where(I => I.Id == idI).FirstOrDefault();
                                        int idS = _context.Symptoms.Where(S => S.Name.Equals(row.Cell(map["симптом"]).Value.ToString())).FirstOrDefault().Id;
                                        newIS.Symptom = _context.Symptoms.Where(S => S.Id == idS).FirstOrDefault();
                                        newIS.IllnessId = idI;
                                        newIS.SymptomId = idS;
                                        newIS.Frequency = Convert.ToInt32(row.Cell(map["частота"]).Value.ToString());
                                        if (newIS.Frequency <= 0 || newIS.Frequency > 100)
                                            throw new Exception("Frequency is not in range [1, 100]");

                                        var d = _context.IllnessSymptoms.Where(IS => IS.IllnessId == newIS.IllnessId && IS.SymptomId == newIS.SymptomId).ToList();
                                        if (d.Count == 0)
                                        {
                                            _context.IllnessSymptoms.Add(newIS);
                                            await _context.SaveChangesAsync();
                                        }

                                        else
                                            foreach (IllnessSymptom IS in d)
                                            {
                                                IS.Frequency = newIS.Frequency;
                                                await _context.SaveChangesAsync();
                                            }
                                                
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var illnesses = _context.Illnesses.ToList();
                foreach (var i in illnesses)
                {
                    var worksheet = workbook.Worksheets.Add(i.Name);

                    worksheet.Cell("A1").Value = "Симптом";
                    worksheet.Cell("B1").Value = "Частота";
                    var symptoms = _context.IllnessSymptoms.Where(il => il.Illness == i).Select(il => il.Symptom).ToList();

                    int j = 2;
                    foreach (var s in symptoms)
                    {
                        worksheet.Cell(j++, 1).Value = s.Name;
                        worksheet.Cell(j - 1, 2).Value = _context.IllnessSymptoms.Where(IS => IS.SymptomId == s.Id && IS.IllnessId == i.Id).FirstOrDefault().Frequency;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Illnesses_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
