using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using varlikHesaplama.DataLayer;
using varlikHesaplama.Hubs;
using varlikHesaplama.Models;

namespace varlikHesaplama.Controllers
{
    public class KurOkumaController : Controller
    {
        private readonly ProjeDbContext _projeDbContext;
        private readonly HttpClient _httpClient;
        private readonly IHubContext<KurHub> _kurDIHub;

        public KurOkumaController(ProjeDbContext projeDbContext, HttpClient httpClient, IHubContext<KurHub> kurDIContext)
        {
            _projeDbContext = projeDbContext;
            _httpClient = httpClient;
            _kurDIHub = kurDIContext;
        }
        public async Task<IActionResult> Index()
        {
            string apiLink = "https://testapi.finmaks.com/ExchangeRates?key=Finmaks123"; // çalışmıyor ?
            //string apiLink = "https://testapi.finmaks.com/ExchangeRates?key=Finmaks123&startDate=2023-09-01&endDate=2023-09-05";

            using (var httpClient = new HttpClient())
            {
                var kur = await httpClient.GetStringAsync(apiLink);
                var okunanKur = JObject.Parse(kur);
                var USD = okunanKur["ExchangeRates"].FirstOrDefault(k => (int)k["BaseCurrencyCode"] == 1); // abd doları

                if (USD != null)
                {
                    var kayitliKur = await _projeDbContext.kurBilgisiTablo.FirstOrDefaultAsync(n => n.Name == "USD");

                    if (kayitliKur != null)
                    {
                        kayitliKur.Tutar = Convert.ToDecimal(USD["CentralBankExchangeRate"]);
                        kayitliKur.Tarih = DateTime.Now;
                    }
                    else
                    {
                        var kurKaydet = new kurBilgisi
                        {
                            Name = "USD",
                            Tutar = Convert.ToDecimal(USD["CentralBankExchangeRate"]),
                            Tarih = DateTime.Now
                        };
                        _projeDbContext.kurBilgisiTablo.Add(kurKaydet);
                    }
                    _projeDbContext.SaveChanges();
                }
                var kurR = new kurSignalR
                {
                    CentralBankExchangeRate = Convert.ToDecimal(USD["CentralBankExchangeRate"])
                };

                await _kurDIHub.Clients.All.SendAsync("ReceiveKur", kurR.CentralBankExchangeRate);

                return View(kurR);
            }
        }
    }
}