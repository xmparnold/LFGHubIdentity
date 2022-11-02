using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LFGHub.Models;
using LFGHub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LFGHub.Controllers;

public class GameActivityController : Controller
{
     private string? uid
    {
        get
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }
    }

    private bool loggedIn
    {
        get
        {
            return User.Identity.IsAuthenticated;
        }
    }

    // db is just a variable name, can be called anything (e.g. DATABASE, db, _db, etc)
    private ApplicationDbContext _context;
    private readonly UserManager<AdvanceUser> _userManager;

     
    // here we can "inject" our context service into the constructor
    public GameActivityController(ApplicationDbContext context, UserManager<AdvanceUser> userManager)
    {
        _context = context;
        _userManager = userManager;

    }




    [HttpGet("/lfg/gameactivities/suggest")]
    public IActionResult New()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        return View("New");
    }

    [HttpPost("/lfg/gameactivities/addsuggestion")]
    public IActionResult Add(GameActivity newActivity)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        newActivity.Approved = false;

        if (ModelState.IsValid == false)
        {
            return New();
        }

        // if (uid != null)
        // {
        //     newActivity.Id = uid;
        // }

        _context.GameActivities.Add(newActivity);
        _context.SaveChanges();

        return RedirectToAction("Dashboard", "Post");
    }


    [HttpGet("/lfg/gameactivities/suggestions")]
    public IActionResult Suggestions()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        // if (!User.IsInRole("Admin"))
        // {
        //     return RedirectToAction("Dashboard", "Post");
        // }

        List<GameActivity> allActivities = _context.GameActivities
            // .Include(ga => ga.SuggestedBy)
            .ToList();

        return View("Suggestions", allActivities);
    }

    // [Authorize(Roles="Admin")]
    [HttpPost("/lfg/gameactivities/{gameActivityId}/approve")]
    public IActionResult Approve(int gameActivityId)
    {
        GameActivity? activity = _context.GameActivities.FirstOrDefault(ga => ga.GameActivityId == gameActivityId);
        
        if (activity == null) //|| !User.IsInRole("Admin"))
        {
            return RedirectToAction("Dashboard", "Post");
        }

        if (activity.Approved)
        {
            activity.Approved = false;

        }
        else
        {
            activity.Approved = true;
        }

        _context.Update(activity);
        _context.SaveChanges();

        return Suggestions();
    }
}