using System.Data;
using ContactManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MariaDB");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (connectionString!.StartsWith("Data Source="))
        options.UseSqlite(connectionString);
    else
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contacts}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetService<ApplicationDbContext>();

    if (dbContext != null && dbContext.Database.GetDbConnection().State != ConnectionState.Open)
    {
        dbContext.Database.OpenConnection();
        dbContext.Database.EnsureCreated();
    }

    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    const string adminEmail = "admin@contacts.pt";
    const string adminPassword = "Admin@123";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(admin, adminPassword);
    }
}

app.Run();
