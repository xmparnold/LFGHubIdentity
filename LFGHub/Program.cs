using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LFGHub.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Certificate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddAuthentication(
//         CertificateAuthenticationDefaults.AuthenticationScheme)
//     .AddCertificate();

// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();
// builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<AdvanceUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders().AddDefaultUI().AddRoles<IdentityRole>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<UserManager<AdvanceUser>>();

var app = builder.Build();

var serviceProvider = app.Services.GetService<IServiceProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
// app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// async Task CreateRoles(IServiceProvider serviceProvider)
// {
//     //initializing custom roles 
//     var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//     var UserManager = serviceProvider.GetRequiredService<UserManager<AdvanceUser>>();
//     string[] roleNames = { "Admin", "Member" };
//     IdentityResult roleResult;

//     foreach (var roleName in roleNames)
//     {
//         var roleExist = await RoleManager.RoleExistsAsync(roleName);
//         // ensure that the role does not exist
//         if (!roleExist)
//         {
//             //create the roles and seed them to the database: 
//             roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
//         }
//     }

//     // find the user with the admin email 
//     var _user = await UserManager.FindByEmailAsync("admin@email.com");

//     // check if the user exists
//     if(_user == null)
//     {
//         //Here you could create the super admin who will maintain the web app
//         var poweruser = new AdvanceUser
//         {
//             UserName = "admin@email.com",
//             Email = "admin@email.com",
//         };
//         string adminPassword = "password";

//         var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword);
//         if (createPowerUser.Succeeded)
//         {
//             //here we tie the new user to the role
//             await UserManager.AddToRoleAsync(poweruser, "Admin");

//         }
//     }
// }


