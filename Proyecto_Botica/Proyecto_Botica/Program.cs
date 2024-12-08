using Proyecto_Botica.Repositorio;
using Proyecto_Botica.Repositorio.RepositorioSQL;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de sesión
builder.Services.AddDistributedMemoryCache(); // Requerido para sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Seguridad: solo accesible a través de HTTP
    options.Cookie.IsEssential = true; // Necesario para que funcione sin consentimiento de cookies
});

builder.Services.AddSingleton<IProducto, productoSQL>();
builder.Services.AddSingleton<ICategoria, categoriaSQL>();
builder.Services.AddSingleton<IVenta, ventaSQL>();
builder.Services.AddSingleton<IDetalleVenta, detVentaSQL>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Habilitar middleware de sesión
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Venta}/{action=ListarProductosVenta}/{id?}");

app.Run();
