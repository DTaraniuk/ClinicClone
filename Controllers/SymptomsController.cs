using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;

namespace ClinicWebApplication.Controllers
{
    public class SymptomsController : Controller
    {
        public IActionResult CheckSymptomName(string name)
        {
            foreach (var s in _context.Symptoms.ToList())
                if (Equalizer.CompareStrings(s.Name, name))
                    return Json(false);
            return Json(true);
        }

        private readonly DBClinicContext _context;

        public SymptomsController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: Symptoms
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (name == null)
                return View(await _context.Symptoms.ToListAsync());

            ViewBag.IllnessId = id;
            ViewBag.IllnessName = name;
            var symptomsBySymptom = _context.IllnessSymptoms.Where(IS => IS.IllnessId == id).Select(IS => IS.Symptom);
            return View(await symptomsBySymptom.ToListAsync());
        }

        // GET: Symptoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptom = await _context.Symptoms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (symptom == null)
            {
                return NotFound();
            }

            return RedirectToAction("IndexBySymptom", "IllnessSymptoms", new { id = symptom.Id, name = symptom.Name });
        }

        // GET: Symptoms/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateIS(int illnessId)
        {
            return RedirectToAction("Create", "IllnessSymptoms", new { illnessId = illnessId, symptomId = -1 });
        }

        // POST: Symptoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Symptom symptom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(symptom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(symptom);
        }

        // GET: Symptoms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var symptom = await _context.Symptoms.FindAsync(id);
            if (symptom == null)
            {
                return NotFound();
            }
            return View(symptom);
        }

        // POST: Symptoms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Symptom symptom)
        {
            if (id != symptom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(symptom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SymptomExists(symptom.Id))
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
            return View(symptom);
        }

        // GET: Symptoms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptom = await _context.Symptoms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (symptom == null)
            {
                return NotFound();
            }

            return View(symptom);
        }

        // POST: Symptoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var illnessSymptoms = _context.IllnessSymptoms.Where(IS => IS.SymptomId == id);
            _context.IllnessSymptoms.RemoveRange(illnessSymptoms);
            var patientSymptoms = _context.PatientSymptoms.Where(ps => ps.SymptomId == id);
            _context.PatientSymptoms.RemoveRange(patientSymptoms);

            var symptom = await _context.Symptoms.FindAsync(id);
            _context.Symptoms.Remove(symptom);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SymptomExists(int id)
        {
            return _context.Symptoms.Any(e => e.Id == id);
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
                                Dictionary<String, int> map = new Dictionary<String,int>();
                                
                                for (int i=1; i<=10; ++i)
                                {
                                    if (worksheet.Cell(1, i) == null) continue;
                                    map[worksheet.Cell(1, i).Value.ToString().ToLower()] = i;
                                }
                                if (!map.ContainsKey("частота") || !map.ContainsKey("хвороба"))
                                {
                                    continue;
                                }
                                await Task.Run(() => (addSymptom(worksheet.Name)));
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        IllnessSymptom newIS = new IllnessSymptom();
                                        await Task.Run(() => (addIllness(row.Cell(map["хвороба"]).Value.ToString())));
                                        

                                        int idI = _context.Illnesses.Where(I => I.Name.Equals(row.Cell(map["хвороба"]).Value.ToString())).FirstOrDefault().Id;
                                        newIS.Illness = _context.Illnesses.Where(I => I.Id == idI).FirstOrDefault();
                                        int idS = _context.Symptoms.Where(S => S.Name.Equals(worksheet.Name)).FirstOrDefault().Id;
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
                var symptoms = _context.Symptoms.ToList();
                foreach (var s in symptoms)
                {
                    var worksheet = workbook.Worksheets.Add(s.Name);

                    worksheet.Cell("A1").Value = "Хвороба";
                    worksheet.Cell("B1").Value = "Частота";
                    var illnesses = _context.IllnessSymptoms.Where(IS => IS.Symptom == s && IS.Illness!=null).Select(IS => IS.Illness).ToList();

                    int j = 2;
                    foreach (var i in illnesses)
                    {
                        worksheet.Cell(j++, 1).Value = i.Name;
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
                        FileDownloadName = $"Symptoms_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
