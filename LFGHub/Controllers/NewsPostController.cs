using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LFGHub.Models;
using LFGHub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LFGHub.Controllers;

public class NewsPostController : Controller
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
    public NewsPostController(ApplicationDbContext context, UserManager<AdvanceUser> userManager)
    {
        _context = context;
        _userManager = userManager;

    }

    [Authorize(Roles="Admin")]
    [HttpGet("/newsposts/new")]
    public IActionResult New()
    {
        if (!User.IsInRole("Admin"))
        {
            if (!loggedIn)
            {
                return RedirectToAction("Index", "User");
            }
            return RedirectToAction("Dashboard", "Post");
        }

        return View("New");
    }

    [Authorize(Roles="Admin")]
    [HttpPost("/newsposts/create")]
    public IActionResult Create(NewsPost newPost)
    {
        if (!User.IsInRole("Admin"))
        {
            return RedirectToAction("Index", "User");
        }

        if (ModelState.IsValid == false)
        {
            return New();
        }

        _context.NewsPosts.Add(newPost);
        _context.SaveChanges();
        
        // return ViewNewsPost(newPost.NewsPostId);
        return RedirectToAction("ViewNewsPost", new { newsPostId = newPost.NewsPostId });

    }

    [HttpGet("/newsposts/{newsPostId}/view")]
    public IActionResult ViewNewsPost(string newsPostId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        NewsPost? post = _context.NewsPosts.FirstOrDefault(np => np.NewsPostId == newsPostId);

        if (post == null)
        {
            return RedirectToAction("Dashboard", "Post");
        }

        return View("View", post);
    }
    [Authorize(Roles="Admin")]
    [HttpGet("/newsposts/{newsPostId}/edit")]
    public IActionResult Edit(string newsPostId)
    {
        if (!User.IsInRole("Admin"))
        {
            if (!loggedIn)
            {
                return RedirectToAction("Index", "User");
            }
            return RedirectToAction("Dashboard", "Post");
        }

        NewsPost? post = _context.NewsPosts.FirstOrDefault(np => np.NewsPostId == newsPostId);

        if (post == null)
        {
            return RedirectToAction("Dashboard", "Post");
        }

        return View("Edit", post);
    }

    [Authorize(Roles="Admin")]
    [HttpPost("/newsposts/{newsPostId}/update")]
    public IActionResult Update(string newsPostId, NewsPost updatedNewsPost)
    {
        if (!User.IsInRole("Admin"))
        {
            if (!loggedIn)
            {
                return RedirectToAction("Index", "User");
            }
            return RedirectToAction("Dashboard", "Post");
        }

        if (ModelState.IsValid == false)
        {
            return Edit(newsPostId);
        }

        NewsPost? dbPost = _context.NewsPosts.FirstOrDefault(np => np.NewsPostId == newsPostId);

        if (dbPost == null)
        {
            return RedirectToAction("Dashboard", "Post");
        }

        dbPost.Title = updatedNewsPost.Title;
        dbPost.Subtitle = updatedNewsPost.Subtitle;
        dbPost.ImageUrl = updatedNewsPost.ImageUrl;
        dbPost.SmallImageUrl = updatedNewsPost.SmallImageUrl;
        dbPost.Text = updatedNewsPost.Text;
        dbPost.UpdatedAt = DateTime.Now;
        _context.NewsPosts.Update(dbPost);
        _context.SaveChanges();

        return RedirectToAction("ViewNewsPost", new { newsPostId = dbPost.NewsPostId });

    }
}