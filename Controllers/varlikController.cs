using Microsoft.AspNetCore.Mvc;
using varlikHesaplama.DataLayer;
using varlikHesaplama.Models;

namespace varlikHesaplama.Controllers
{
    public class varlikController : Controller
    {
        private readonly ProjeDbContext _projeDbContext;

        public varlikController(ProjeDbContext projeDbContext)
        {
            _projeDbContext = projeDbContext;
        }

        public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(varlik v)
        {
            if (ModelState.IsValid) 
            {
                varlik varlik1 = new varlik()
                {
                    Name = v.Name,
                    Tutari = v.Tutari,
                    Tarihi = v.Tarihi
                };
                _projeDbContext.varlikTablo.Add(varlik1);
            }
            _projeDbContext.SaveChanges();

            return RedirectToAction("Save", "varlik");
        }

        public IActionResult List() 
        {
            List<varlik> varlikList = _projeDbContext.varlikTablo.ToList();
            return View(varlikList);
        }

        public IActionResult Update(int? id)
        {
            var varlik = _projeDbContext.varlikTablo.FirstOrDefault(v => v.Id == id);
            return View(varlik);
        }

        [HttpPost]
        public IActionResult Update(varlik varlik2)
        {
            var gelenVarlik = _projeDbContext.varlikTablo.FirstOrDefault(v => v.Id == varlik2.Id);

            if (gelenVarlik == null)
            {
                return NotFound();
            }

            gelenVarlik.Name = varlik2.Name;
            gelenVarlik.Tutari = varlik2.Tutari;
            gelenVarlik.Tarihi = varlik2.Tarihi;

            _projeDbContext.SaveChanges();

            return RedirectToAction("List", "varlik");
        }

        public IActionResult Delete(int? id) 
        {
            var varlikS = _projeDbContext.varlikTablo.FirstOrDefault(v =>v.Id == id);

            if (varlikS == null) 
            { 
                return NotFound();
            }

            _projeDbContext.Remove(varlikS);

            _projeDbContext.SaveChanges();

            return RedirectToAction("List", "varlik");
        }
    }
}