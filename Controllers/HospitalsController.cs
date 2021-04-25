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
    public class HospitalsController : Controller
    {
        private readonly DBClinicContext _context;

        public HospitalsController(DBClinicContext context)
        {
            _context = context;
        }

        // GET: Hospitals
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "Cities");
            if (name == null)
                name = _context.Cities.Where(c => c.Id == id).FirstOrDefault().Name;
            ViewBag.CityId = id;
            ViewBag.CityName = name;
            var hospitalsByCity = _context.Hospitals.Where(h => h.CityId == id).Include(h => h.City);
            return View(await hospitalsByCity.ToListAsync());
        }

        // GET: Hospitals/Details/5
        public async Task<IActionResult> Details(int? id, int? cityId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals
                .Include(h => h.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return NotFound();
            }
            ViewBag.cityId = cityId;
            ViewBag.HospitalName = hospital.Name;
            ViewBag.HospitalId = hospital.Id;
            return RedirectToAction("Index", "Departments", new {id = hospital.Id, name = hospital.Name, cityId = cityId});
        }

        // GET: Hospitals/Create
        public IActionResult Create(int cityId)
        {
            ViewBag.CityId = cityId;
            ViewBag.CityName = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault().Name;
            //ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            return View();
        }

        // POST: Hospitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CityId,Address")] Hospital hospital, int? cityId)//
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospital);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Hospitals", new { id = cityId });
            }
            ViewBag.cityIdX = cityId;
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", hospital.CityId);
            return View(hospital);
        }

        // GET: Hospitals/Edit/5
        public async Task<IActionResult> Edit(int? id, int? cityId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            ViewBag.cityIdX = cityId;
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", hospital.CityId);
            return View(hospital);
        }

        // POST: Hospitals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CityId,Address")] Hospital hospital, int? cityId)
        {
            if (id != hospital.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospital);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalExists(hospital.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Hospitals", new { id = cityId });
            }
            ViewBag.cityIdX = cityId;
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", hospital.CityId);
            return View(hospital);
        }

        // GET: Hospitals/Delete/5
        public async Task<IActionResult> Delete(int? id, int? cityId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals
                .Include(h => h.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return NotFound();
            }

            ViewBag.cityId = cityId;
            return View(hospital);
        }

        // POST: Hospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id,int? cityId)
        {
            var departments = _context.Departments.Where(d => d.HospitalId == id);

            var dc = new DepartmentsController(_context);
            foreach (Department d in departments.ToList())
                await dc.DeleteConfirmed(d.Id);

            var hospital = await _context.Hospitals.FindAsync(id);
            _context.Hospitals.Remove(hospital);
            await _context.SaveChangesAsync();
            ViewBag.cityId = cityId;
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalExists(int id)
        {
            return _context.Hospitals.Any(e => e.Id == id);
        }
        async Task AddDepartment(String s, int hospitalId)
        {
            int idD = -1;
            try
            {
                idD = _context.Departments.Where(I => I.Name.Equals(s) && I.HospitalId == hospitalId).FirstOrDefault().Id;
            }
            catch (Exception e)
            {
                Department d = new Department();
                d.Name = s;
                d.HospitalId = idD;
                d.Hospital = _context.Hospitals.Where(h => h.Id == hospitalId).FirstOrDefault();
                _context.Departments.Add(d);
                await _context.SaveChangesAsync();
            }
        }
        async Task AddDoctor(String s, String departamentName, String hospitalName)
        {
            Department dep = _context.Departments.Where(d => d.Name.Equals(departamentName) && d.Hospital.Name==hospitalName).FirstOrDefault();
            int idDoc = -1;
            try
            {
                var doc = _context.Doctors.Where(I => I.Name.Equals(s)).FirstOrDefault();
                if (doc.Name == doc.Department.DepartmentHead.Name) doc.Department.DepartmentHead = null;
                doc.Department = dep;
                doc.DepartmentId = dep.Id;
            }
            catch (Exception e)
            {
                Doctor d = new Doctor();
                d.Name = s;
                d.Department = dep;
                d.DepartmentId = dep.Id;
                _context.Doctors.Add(d);
                await _context.SaveChangesAsync();
            }
        }

        async Task AddPatient(String s, String doctorName)
        {
            Doctor doc = _context.Doctors.Where(d => d.Name.Equals(doctorName)).FirstOrDefault();
            int idPat = -1;
            try
            {
                var pat = _context.Patients.Where(p => p.Name.Equals(s)).FirstOrDefault();
                pat.Therapist = doc;
                pat.TherapistId = doc.Id;
            }
            catch (Exception e)
            {
                Patient p = new Patient();
                p.Name = s;
                p.Therapist = doc;
                p.TherapistId = doc.Id;
                _context.Patients.Add(p);
                await _context.SaveChangesAsync();
            }
        }
        async Task AddHospital(String s, int cityId)
        {
            City city = _context.Cities.Where(c => c.Id == cityId).FirstOrDefault();
            int idHos = -1;
            try
            {
                idHos = _context.Hospitals.Where(h=>h.Name.Equals(s) && h.CityId==cityId).FirstOrDefault().Id;
            }
            catch (Exception e)
            {
                Hospital h = new Hospital();
                h.City = city;
                h.CityId = cityId;
                h.Name = s;

                _context.Hospitals.Add(h);
                await _context.SaveChangesAsync();
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel, String name, int cityId)
        {
            await Task.Run(() => (AddHospital(name,cityId)));
            int id = _context.Hospitals.Where(h => h.Name == name).FirstOrDefault().Id;

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
                                Dictionary<String, int> map = new Dictionary<String, int>();

                                for (int i = 1; i <= 10; ++i)
                                {
                                    if (worksheet.Cell(1, i) == null) continue;
                                    map[worksheet.Cell(1, i).Value.ToString().ToLower()] = i;
                                }
                                if(!map.ContainsKey("лікар") || !map.ContainsKey("пацієнт"))
                                    {
                                    continue;
                                }

                                string departamnetName = worksheet.Name;
                                await Task.Run(() => (AddDepartment(departamnetName, id)));
                                String prevDocName = null;
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        string doctorName = row.Cell(map["лікар"]).Value.ToString();
                                        if (doctorName == "") doctorName = prevDocName;
                                        await Task.Run(() => (AddDoctor(doctorName, departamnetName,name)));
                                        try
                                        {
                                            string patientName = row.Cell(map["пацієнт"]).Value.ToString();
                                            await Task.Run(() => (AddPatient(patientName, doctorName)));
                                        }
                                        catch (Exception e)
                                        {
                                            //logging самостійно :)

                                        }

                                        prevDocName = doctorName;
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
            return RedirectToAction("Index", new { id = cityId });
        }
        public ActionResult Export(int id)
        {
            try
            {
                using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
                {
                    var departments = _context.Departments.Where(dep => dep.HospitalId == id).ToList();
                    //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                    foreach (var d in departments)
                    {
                        var worksheet = workbook.Worksheets.Add(d.Name);

                        worksheet.Cell("A1").Value = "Лікар";
                        worksheet.Cell("B1").Value = "Пацієнт";
                        worksheet.Column(1).Width = 25;
                        worksheet.Column(2).Width = 25;
                        //worksheet.Column(1).AsRange().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        //worksheet.Column(1).AsRange().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        //worksheet.Column(2).AsRange().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        //worksheet.Column(2).AsRange().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                        /*
                        worksheet.Cell("B1").Value = "Автор 1";
                        worksheet.Cell("C1").Value = "Автор 2";
                        worksheet.Cell("D1").Value = "Автор 3";
                        worksheet.Cell("E1").Value = "Автор 4";
                        worksheet.Cell("F1").Value = "Категорія";
                        worksheet.Cell("G1").Value = "Інформація";
                        worksheet.Row(1).Style.Font.Bold = true;*/

                        //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                        int j = 2;
                        List<Doctor> doctors = _context.Doctors.Where(Doc => Doc.DepartmentId == d.Id).ToList();
                        //List<Patient> usedPatients = _context.Patients.Where(P => (doctors.Any(D => (D.Id == P.TherapistId)))).ToList();
                        List<Patient> usedPatients = _context.Patients.ToList();
                        int width = usedPatients.Count + doctors.Count + 1; 

                        var w=worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(width, 2));
                        w.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        w.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        foreach (var doc in doctors)
                        {
                            var patiens = _context.Patients.Where(pat => pat.TherapistId == doc.Id).ToList();

                            if (patiens.Count == 0) patiens.Add(new Patient());
                            int st = j;
                            foreach (var pat in patiens)
                            {
                                worksheet.Cell(j++, 1).Value = doc.Name;
                                worksheet.Cell(j - 1, 2).Value = pat.Name;
                            }
                            worksheet.Range(worksheet.Cell(st, 1), worksheet.Cell(j - 1, 1)).Merge();
                            worksheet.Cell(st, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(st, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            /*
                            worksheet.Cell(i + 2, 7).Value = books[i].Info;

                            var ab = _context.AuthorsBooks.Where(a => a.BookId == books[i].Id).Include("Author").ToList();
                            //більше 4-ох нікуди писати
                            int j = 0;
                            foreach (var a in ab)
                            {
                                if (j < 5)
                                {
                                    worksheet.Cell(i + 2, j + 2).Value = a.Author.Name;
                                    j++;
                                }
                            }*/

                        }
                    }
                    if (workbook.Worksheets.Count == 0)
                    {
                        var worksheet = workbook.Worksheets.Add("ПОМИЛКА");
                        worksheet.Cell("A1").Value = "В цій лікарні нема відділів, спробуйте пізніше";
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Flush();

                        return new FileContentResult(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            FileDownloadName = $"Clinic_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return null;

            }
        }
    }
}
