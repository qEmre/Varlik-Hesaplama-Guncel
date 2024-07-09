using Microsoft.AspNetCore.Mvc;
using varlikHesaplama.DataLayer;
using varlikHesaplama.Models;

namespace varlikHesaplama.Controllers
{
    public class hesaplamaController : Controller
    {
        private readonly ProjeDbContext _projeDbContext;

        public hesaplamaController(ProjeDbContext projeDbContext)
        {
            _projeDbContext = projeDbContext;
        }

        [HttpPost]
        public IActionResult Index(List<int> varliklar)
        {
            List<varlik> gelenVarlik = _projeDbContext.varlikTablo.Where(v => varliklar.Contains(v.Id)).ToList();
            List<ufeEndeks> endeksVerileri = _projeDbContext.ufeEndeksTablo.ToList();
            List<hesaplanmisDegerler> hesaplanmisOranlar = new List<hesaplanmisDegerler>();

            foreach (var item in gelenVarlik)
            {
                // varlığın tarihi üfe endeksi ve dolar kuru
                var firstEndeks = _projeDbContext.ufeEndeksTablo.OrderBy(e => e.Tarih).FirstOrDefault(e => e.Tarih == item.Tarihi);
                var firstKur = _projeDbContext.ufeEndeksTablo.OrderBy(e => e.Tarih).FirstOrDefault(e => e.Tarih == item.Tarihi);

                // rapor tarihi üfe endeksi ve dolar kuru 
                var lastEndeks = _projeDbContext.ufeEndeksTablo.OrderByDescending(e => e.Tarih).FirstOrDefault();
                var lastkur = _projeDbContext.ufeEndeksTablo.OrderByDescending(e => e.Tarih).FirstOrDefault();

                // simdiki ve onceki ayın varlık verilerini çek
                var simdikiAy = item.Tarihi;
                var oncekiAy = simdikiAy.AddMonths(-1);
                var simdikiAyVarlik = _projeDbContext.varlikTablo.FirstOrDefault(v => varliklar.Contains(v.Id) && v.Tarihi == simdikiAy);
                var oncekiAyVarlik = _projeDbContext.varlikTablo.FirstOrDefault(v => varliklar.Contains(v.Id) && v.Tarihi == oncekiAy);

                var lastVarlikTutari = _projeDbContext.varlikTablo.Where(v => varliklar.Contains(v.Id)).OrderByDescending(v => v.Tarihi).FirstOrDefault();
                
                // onceki ayın kur bilgileri
                var varlikDolKur = _projeDbContext.ufeEndeksTablo.OrderBy(k => k.Tarih).FirstOrDefault(k => k.Tarih == oncekiAy);
                var varlikEnfKur = _projeDbContext.ufeEndeksTablo.OrderBy(k => k.Tarih).FirstOrDefault(k => k.Tarih == oncekiAy);

                foreach (var item1 in endeksVerileri)
                {
                    var enflasyonVarlikTutari = (lastEndeks.Deger * item.Tutari) / firstEndeks.Deger;
                    var dolarizasyonVarlikTutari = (lastkur.DolarKuru * item.Tutari) / firstKur.DolarKuru;

                    decimal oncekiAyaGoreArtisVarlik = 0;
                    if (oncekiAyVarlik != null)
                    {
                        oncekiAyaGoreArtisVarlik = ((simdikiAyVarlik.Tutari - oncekiAyVarlik.Tutari) / oncekiAyVarlik.Tutari) * 100;
                    }

                    decimal varlikDegisimOrani = (lastVarlikTutari.Tutari / simdikiAyVarlik.Tutari) * 100 - 100;

                    decimal dolarizasyonOncekiAyaGoreVarlikArtis = 0;
                    if (oncekiAyVarlik != null)
                    {
                        var oncekiDolarizasyonTutari = (lastkur.DolarKuru * oncekiAyVarlik.Tutari) / varlikDolKur.DolarKuru;
                        dolarizasyonOncekiAyaGoreVarlikArtis = (dolarizasyonVarlikTutari - oncekiDolarizasyonTutari) / oncekiDolarizasyonTutari * 100;
                    }

                    decimal dolarizasyonEtkisiYuzde = (simdikiAyVarlik.Tutari / dolarizasyonVarlikTutari) * 100 - 100;
                    decimal enflasyonEtkisiYüzde = (simdikiAyVarlik.Tutari / enflasyonVarlikTutari) * 100 - 100;

                    decimal enflasyonOncekiAyaGoreVarlikArtis = 0;
                    if (oncekiAyVarlik != null)
                    {
                        var oncekiEnflasyonTutari = (lastEndeks.Deger * oncekiAyVarlik.Tutari) / varlikEnfKur.Deger;
                        enflasyonOncekiAyaGoreVarlikArtis = (enflasyonVarlikTutari - oncekiEnflasyonTutari) / oncekiEnflasyonTutari * 100;
                    }

                    hesaplanmisOranlar.Add(new hesaplanmisDegerler
                    {
                        Tarih = item.Tarihi,
                        varlikTutari = item.Tutari,
                        varlikTarihiDolarKuru = firstKur.DolarKuru,
                        ufeEndeks = firstKur.Deger,
                        enflasyonVarliktutari = enflasyonVarlikTutari,
                        dolarizasyonVarlikTutari = dolarizasyonVarlikTutari,
                        oncekiAyaGoreVarlikArtis = oncekiAyaGoreArtisVarlik,
                        varlikDegisimOrani = varlikDegisimOrani,
                        dolarizasyonOncekiAyaGoreVarlikArtis = dolarizasyonOncekiAyaGoreVarlikArtis,
                        dolarizasyonEtkisiYüzde = dolarizasyonEtkisiYuzde,
                        enflasyonOncekiAyaGoreVarlikArtis = enflasyonOncekiAyaGoreVarlikArtis,
                        enflasyonEtkisiYuzde = enflasyonEtkisiYüzde,
                    });
                    break;
                }
            }
            var enflasyonVarlikTutari1 = hesaplanmisOranlar.OrderByDescending(e => e.Tarih).FirstOrDefault().enflasyonVarliktutari;
            var dolarizasyonVarlikTutari1 = hesaplanmisOranlar.OrderByDescending(d => d.Tarih).FirstOrDefault().dolarizasyonVarlikTutari;

            foreach (var item2 in hesaplanmisOranlar)
            {
                decimal enflasyonVarlikDegisimOrani = 0;
                enflasyonVarlikDegisimOrani = (enflasyonVarlikTutari1 / item2.enflasyonVarliktutari) * 100 - 100;
                item2.enflasyonVarlikDegisimOrani = enflasyonVarlikDegisimOrani;

                decimal dolarizasyonVarlikDegisimOrani = 0;
                dolarizasyonVarlikDegisimOrani = (dolarizasyonVarlikTutari1 / item2.dolarizasyonVarlikTutari) * 100 - 100;
                item2.dolarizasyonVarlikDegisimOrani = dolarizasyonVarlikDegisimOrani;
            }
            return View(hesaplanmisOranlar);
        }
    }
}