using Microsoft.AspNetCore.Mvc;
using varlikHesaplama.DataLayer;
using varlikHesaplama.Models;

namespace varlikHesaplama.Controllers
{
    public class ufeEndeksController : Controller
    {
        private readonly ProjeDbContext _projeDbContext;

        public ufeEndeksController(ProjeDbContext projeDbContext)
        {
            _projeDbContext = projeDbContext;
        }

        public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(ufeEndeks e)
        {
            if (ModelState.IsValid)
            {
                ufeEndeks endeks = new ufeEndeks()
                {
                    Deger = e.Deger,
                    Tarih = e.Tarih,
                    DolarKuru = e.DolarKuru
                };
                _projeDbContext.ufeEndeksTablo.Add(endeks);
            }
            _projeDbContext.SaveChanges();

            return RedirectToAction("Save", "ufeEndeks");
        }

        public IActionResult Update(int? id)
        {
            var endeksG = _projeDbContext.ufeEndeksTablo.FirstOrDefault(e => e.Id == id);

            return View(endeksG);
        }

        [HttpPost]
        public IActionResult Update(ufeEndeks endeks)
        {
            var gelenEndeks = _projeDbContext.ufeEndeksTablo.FirstOrDefault(e => e.Id == endeks.Id);
            
            if (gelenEndeks == null)
            {
                return NotFound();
            }

            gelenEndeks.Deger = endeks.Deger;
            gelenEndeks.DolarKuru = endeks.DolarKuru;
            gelenEndeks.Tarih = endeks.Tarih;

            _projeDbContext.SaveChanges();

            return RedirectToAction("List", "ufeEndeks");
        }

        public IActionResult Delete(int? id) 
        {
            var endeksSil = _projeDbContext.ufeEndeksTablo.FirstOrDefault(e => e.Id == id);

            if (endeksSil == null)
            {
                return NotFound();
            }
            _projeDbContext.Remove(endeksSil);
            _projeDbContext.SaveChanges();
            return RedirectToAction("List", "ufeEndeks");
        }

        public IActionResult List() 
        {
            List<ufeEndeks> endeksList = _projeDbContext.ufeEndeksTablo.ToList();
            return View(endeksList);
        }
    }
}