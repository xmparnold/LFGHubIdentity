using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LFGHub.Models;
using LFGHub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LFGHub.Controllers;

public class GameController : Controller
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

    private ApplicationDbContext _context;
     
    // here we can "inject" our context service into the constructor
    public GameController(ApplicationDbContext context)
    {
        _context = context;
    }
}