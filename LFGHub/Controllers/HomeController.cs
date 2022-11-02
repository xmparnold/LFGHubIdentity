using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LFGHub.Models;
using Microsoft.AspNetCore.Identity;

namespace LFGHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AdvanceUser> _userManager;

    public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider, RoleManager<IdentityRole> roleManager, UserManager<AdvanceUser> userManager)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Dashboard", "Post");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    private async Task CreateRolesandUsers()
    {  
        bool x = await _roleManager.RoleExistsAsync("Admin");
        if (!x)
        {
            // first we create Admin rool    
            var role = new IdentityRole();
            role.Name = "Admin";
            await _roleManager.CreateAsync(role);

            //Here we create a Admin super user who will maintain the website                   

            var user = new AdvanceUser();
            user.UserName = "admin@email.com";
            user.Email = "admin@email.com";

            string userPWD = "password";

            IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

            //Add default User to Role Admin    
            if (chkUser.Succeeded)
            {
                var result1 = await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }

}
