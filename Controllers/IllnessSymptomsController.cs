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
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;

namespace ClinicWebApplication.Controllers
{
    public class IllnessSymptomsController : Controller
    {
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckSymptomName(string name)
        {
            if (_context.Symptoms.Where(s => s.Name == name).Count() > 0)
                return Json(false);
            return Json(true);
        }

        private readonly DBClinicContext _context;

        public IllnessSymptomsController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: IllnessSymptoms
        public async Task<IActionResult> Index()
        {
            var dBClinicContext = _context.IllnessSymptoms.Include(i => i.Illness).Include(i => i.Symptom);
            return View(await dBClinicContext.ToListAsync());
        }
        public async Task<IActionResult> IndexBySymptom(int? id, string? name)
        {
            ViewBag.FromSymptom = true;
            ViewBag.SymptomId = id;
            ViewBag.SymptomName = name;

            var dBClinicContext = _context.IllnessSymptoms.Where(i => i.SymptomId == id).Include(i => i.Illness).Include(i => i.Symptom);
            return View(await dBClinicContext.ToListAsync());
        }

        public async Task<IActionResult> IndexByIllness(int? id, string? name)
        {
            ViewBag.FromIllness = true;
            ViewBag.IllnessId = id;
            ViewBag.IllnessName = name;

            var dBClinicContext = _context.IllnessSymptoms.Where(i => i.IllnessId == id).Include(i => i.Illness).Include(i => i.Symptom);
            return View(await dBClinicContext.ToListAsync());
        }


        // GET: IllnessSymptoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var illnessSymptom = await _context.IllnessSymptoms
                .Include(i => i.Illness)
                .Include(i => i.Symptom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (illnessSymptom == null)
            {
                return NotFound();
            }

            return View(illnessSymptom);
        }

        // GET: IllnessSymptoms/Create
        public IActionResult Create(int illnessId, int symptomId)
        {
            ViewBag.IllnessChosenId = illnessId;
            if (illnessId != -1)
            {
                ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Name");
                ViewBag.IllnessName = _context.Illnesses.Where(i => i.Id == illnessId).FirstOrDefault().Name;
            }
            ViewBag.SymptomChosenId = symptomId;
            if (symptomId != -1)
            {
                ViewData["IllnessId"] = new SelectList(_context.Illnesses, "Id", "Name");
                ViewBag.SymptomName = _context.Symptoms.Where(i => i.Id == symptomId).FirstOrDefault().Name;
            }
            return View();
        }

        // POST: IllnessSymptoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromIllness([Bind("Id,IllnessId,SymptomId,Frequency")] IllnessSymptom illnessSymptom)
        {
            while (ModelState.IsValid)
            {
                _context.Add(illnessSymptom);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexByIllness", "IllnessSymptoms", new
                {
                    id = illnessSymptom.IllnessId,
                    name = _context.Illnesses.Where(i => i.Id == illnessSymptom.IllnessId).FirstOrDefault().Name
                });
            }
            ViewData["IllnessId"] = new SelectList(_context.Illnesses, "Id", "Name", illnessSymptom.IllnessId);
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Name", illnessSymptom.SymptomId);
            return View(illnessSymptom);
        }
        public async Task<IActionResult> CreateFromSymptom([Bind("Id,IllnessId,SymptomId,Frequency")] IllnessSymptom illnessSymptom)
        {
            while (ModelState.IsValid)
            {
                _context.Add(illnessSymptom);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexBySymptom", "IllnessSymptoms", new
                {
                    id = illnessSymptom.SymptomId,
                    name = _context.Symptoms.Where(s => s.Id == illnessSymptom.SymptomId).FirstOrDefault().Name
                });
            }
            ViewData["IllnessId"] = new SelectList(_context.Illnesses, "Id", "Name", illnessSymptom.IllnessId);
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Name", illnessSymptom.SymptomId);
            return View(illnessSymptom);
        }
        // GET: IllnessSymptoms/Edit/5
        public async Task<IActionResult> Edit(int? id, int? illnessId, int? symptomId)
        {
            ViewBag.IllnessChosenId = illnessId;
            ViewBag.SymptomChosenId = symptomId;

            if (illnessId != -1)
                ViewBag.IllnessName = _context.Illnesses.Where(i => i.Id == illnessId).FirstOrDefault().Name;
            if (symptomId != -1)
                ViewBag.SymptomName = _context.Symptoms.Where(s => s.Id == symptomId).FirstOrDefault().Name;

            if (id == null)
            {
                return NotFound();
            }

            var illnessSymptom = await _context.IllnessSymptoms.FindAsync(id);
            if (illnessSymptom == null)
            {
                return NotFound();
            }
            ViewBag.CurrentIllnessId = illnessSymptom.IllnessId;
            ViewBag.CurrentSymptomId = illnessSymptom.SymptomId;
            return View(illnessSymptom);
        }

        // POST: IllnessSymptoms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IllnessId,SymptomId,Frequency")] IllnessSymptom illnessSymptom, int? illnessIdX, int? symptomIdX)
        {
            if (id != illnessSymptom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(illnessSymptom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IllnessSymptomExists(illnessSymptom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (illnessIdX != -1)
                {
                    return RedirectToAction("IndexByIllness", "IllnessSymptoms", new
                    {
                        id = illnessIdX,
                        name = _context.Illnesses.Where(s => s.Id == illnessIdX).FirstOrDefault().Name
                    });
                }
                else
                {
                    return RedirectToAction("IndexBySymptom", "IllnessSymptoms", new
                    {
                        id = symptomIdX,
                        name = _context.Symptoms.Where(s => s.Id == symptomIdX).FirstOrDefault().Name
                    });
                }
            }
            ViewData["IllnessId"] = new SelectList(_context.Illnesses, "Id", "Name", illnessSymptom.IllnessId);
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Name", illnessSymptom.SymptomId);
            return View(illnessSymptom);
        }

        // GET: IllnessSymptoms/Delete/5
        public async Task<IActionResult> Delete(int? id, int? illnessId, int? symptomId)
        {
            ViewBag.IllnessChosenId = illnessId;
            ViewBag.SymptomChosenId = symptomId;
            if (illnessId != -1)
                ViewBag.IllnessName = _context.Illnesses.Where(i => i.Id == illnessId).FirstOrDefault().Name;
            if (symptomId != -1)
                ViewBag.SymptomName = _context.Symptoms.Where(s => s.Id == symptomId).FirstOrDefault().Name;

            if (id == null)
            {
                return NotFound();
            }

            var illnessSymptom = await _context.IllnessSymptoms
                .Include(i => i.Illness)
                .Include(i => i.Symptom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (illnessSymptom == null)
            {
                return NotFound();
            }

            return View(illnessSymptom);
        }

        // POST: IllnessSymptoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? illnessId, int? symptomId)
        {
            var illnessSymptom = await _context.IllnessSymptoms.FindAsync(id);
            _context.IllnessSymptoms.Remove(illnessSymptom);
            await _context.SaveChangesAsync();

            if (illnessId != -1)
            {
                return RedirectToAction("IndexByIllness", "IllnessSymptoms", new
                {
                    id = illnessSymptom.IllnessId,
                    name = _context.Illnesses.Where(s => s.Id == illnessSymptom.IllnessId).FirstOrDefault().Name
                });
            }
            else
            {
                return RedirectToAction("IndexBySymptom", "IllnessSymptoms", new
                {
                    id = illnessSymptom.SymptomId,
                    name = _context.Symptoms.Where(s => s.Id == illnessSymptom.SymptomId).FirstOrDefault().Name
                });
            }
        }

        public async Task<IActionResult> GoToIllnesses()
        {
            return RedirectToAction("Index", "Illnesses");
        }

        public async Task<IActionResult> GoToSymptoms()
        {
            return RedirectToAction("Index", "Symptoms");
        }

        private bool IllnessSymptomExists(int id)
        {
            return _context.IllnessSymptoms.Any(e => e.Id == id);
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
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                /*
                                var c = (from sym in _context.Symptoms
                                         where sym.Name.Contains(worksheet.Name)
                                         select sym).ToList();
                                if (c.Count > 0)
                                {
                                    newIS = c[0];
                                }
                                else
                                {
                                    newSym = new Symptom();
                                    newSym.Name = worksheet.Name;
                                    //newSym. = "from EXCEL";
                                    //додати в контекст
                                    _context.Symptoms.Add(newSym);
                                }*/
                                //перегляд усіх рядків
                                //

                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        IllnessSymptom newIS = new IllnessSymptom();
                                        int idI = _context.Illnesses.Where(I => I.Name == row.Cell(1).Value.ToString()).FirstOrDefault().Id;
                                        newIS.Illness = _context.Illnesses.Where(I => I.Name == row.Cell(1).Value.ToString()).FirstOrDefault();
                                        int idS = _context.Symptoms.Where(S => S.Name == row.Cell(2).Value.ToString()).FirstOrDefault().Id;
                                        newIS.Symptom = _context.Symptoms.Where(S => S.Name == row.Cell(2).Value.ToString()).FirstOrDefault();
                                        newIS.IllnessId = idI;
                                        newIS.SymptomId = idS;
                                        newIS.Frequency = Convert.ToInt32(row.Cell(3).Value.ToString());



                                        //book.Info = row.Cell(6).Value.ToString();
                                        //book.Category = newcat;

                                        var a = _context.IllnessSymptoms.Where(IS => IS.IllnessId == idI && IS.SymptomId == idS).ToList();
                                        if (a.Count == 0)
                                            _context.IllnessSymptoms.Add(newIS);
                                        else
                                            foreach (IllnessSymptom IS in a)
                                                IS.Frequency = newIS.Frequency;
                                        /*
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        for (int i = 2; i <= 5; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Author author;

                                                var a = (from aut in _context.Authors
                                                         where aut.Name.Contains(row.Cell(i).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
                                                {
                                                    author = a[0];
                                                }
                                                else
                                                {
                                                    author = new Author();
                                                    author.Name = row.Cell(i).Value.ToString();
                                                    author.Info = "from EXCEL";
                                                    //додати в контекст
                                                    _context.Add(author);
                                                }
                                                AuthorsBooks ab = new AuthorsBooks();
                                                ab.Book = book;
                                                ab.Author = author;
                                                _context.AuthorsBooks.Add(ab);
                                            }
                                        }*/
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)

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
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                //foreach (var s in symptoms)
                {
                    var worksheet = workbook.Worksheets.Add("Хвороб симптом");

                    worksheet.Cell("A1").Value = "Хвороба";
                    worksheet.Cell("B1").Value = "Симптом";
                    worksheet.Cell("C1").Value = "Частота";

                    var illnessSymptoms = _context.IllnessSymptoms.ToList();
                    int j = 2;
                    foreach (var IS in illnessSymptoms)
                    {
                        worksheet.Cell(j++, 1).Value = IS.Illness == null ? "UNDEF" : IS.Illness.Name ;
                        worksheet.Cell(j - 1, 2).Value = IS.Symptom == null ? "UNDEF" : IS.Symptom.Name;
                        worksheet.Cell(j - 1, 3).Value = IS.Frequency;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
/*
 var worksheet = workbook.Worksheets.Add("Хвороб симптом");

                    worksheet.Cell("A1").Value = "Хвороба";
                    worksheet.Cell("B1").Value = "Симптом";
                    worksheet.Cell("С1").Value = "Частота";
                    int j = 2;
                    foreach (var IS in illnessSymptoms)
                    {
                        worksheet.Cell(j++, 1).Value = IS.Illness.Name;
                        worksheet.Cell(j - 1, 2).Value = IS.Symptom.Name;
                        worksheet.Cell(j - 1, 2).Value = IS.Frequency;
 */