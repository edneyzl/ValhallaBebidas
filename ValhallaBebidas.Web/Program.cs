// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// HttpClient para chamar a API
builder.Services.AddHttpClient("ValhallaAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]
        ?? "http://localhost:5101/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Session para guardar o usuário logado
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();