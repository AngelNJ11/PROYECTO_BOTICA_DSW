using Proyecto_Botica.Repositorio;
using Proyecto_Botica.Repositorio.RepositorioSQL;

var builder = WebApplication.CreateBuilder(args);


// Agregar servicios de sesi�n
builder.Services.AddDistributedMemoryCache(); // Requerido para sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true; // Seguridad: solo accesible a trav�s de HTTP
    options.Cookie.IsEssential = true; // Necesario para que funcione sin consentimiento de cookies
});

builder.Services.AddSingleton<IVenta, ventaSQL>();
builder.Services.AddSingleton<IDetalleVenta, detVentaSQL>();

// Add services to the container.

builder.Services.AddSession();


builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseSession(); // Aseg�rate de que este middleware est� configurado

// Configure the HTTP request pipeline.
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
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();