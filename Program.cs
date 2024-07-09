using Microsoft.EntityFrameworkCore;
using varlikHesaplama.DataLayer;
using varlikHesaplama.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProjeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();
builder.Services.AddHttpClient();

var app = builder.Build();

// HTTP isteði iþleme pipeline'ý yapýlandýrmasý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=KurOkuma}/{action=Index}/{id?}");

app.MapHub<KurHub>("/kurlarHub");

app.Run();
