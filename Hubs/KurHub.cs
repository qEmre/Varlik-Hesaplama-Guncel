using Microsoft.AspNetCore.SignalR;

namespace varlikHesaplama.Hubs
{
    public class KurHub : Hub
    {
        public async Task kurIstek(decimal CentralBankExchangeRate)
        {
            await Clients.All.SendAsync("ReceiveKur", CentralBankExchangeRate);
        }
    }
}