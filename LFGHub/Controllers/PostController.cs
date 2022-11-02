using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LFGHub.Models;
using LFGHub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LFGHub.Controllers;

public class PostController : Controller
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
    public PostController(ApplicationDbContext context, UserManager<AdvanceUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    [HttpGet("/dashboard")]
    public IActionResult Dashboard()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        

        List<NewsPost> allNews = _context.NewsPosts.ToList();
        allNews.Sort(delegate(NewsPost np1, NewsPost np2) { return DateTime.Compare(np2.CreatedAt, np1.CreatedAt); });
        return View("Dashboard", allNews);
    }

    [HttpGet("/lfg/posts/all")]
    public IActionResult All()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        List<Post> allPosts = _context.Posts
        .Include(p => p.Author)
        // .Include(p => p.GroupPlayers)
        // .ThenInclude(groupplayer => groupplayer.User)
        .ToList();
        allPosts.Sort(delegate(Post p1, Post p2) { return DateTime.Compare(p2.CreatedAt, p1.CreatedAt); });
        return View("All", allPosts);
    }

    [HttpGet("/lfg/posts/new")]
    public IActionResult New()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null || !loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        List<GameActivity> allActivities = _context.GameActivities.Where(ga => ga.Approved == true).ToList();
        ViewBag.allActivities = allActivities;

        return View("New");
    }

    // [HttpPost("/lfg/posts/create")]
    // public IActionResult Create(Post newPost)
    // {
    //     if (!loggedIn)
    //     {
    //         return RedirectToAction("Index", "User");
    //     }

    //     if (uid != null)
    //     {
    //         newPost.Id = uid;
    //     }

    //     if (ModelState.IsValid == false)
    //     {
    //         return New();
    //     }
    //     var currentUser = _context.Users.FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
    //     newPost.Author = currentUser;

    //     _context.Posts.Add(newPost);
    //     _context.SaveChanges();

    //     return Dashboard();
    // }

    [HttpPost("/lfg/posts/create")]
    public async Task<IActionResult> Create(Post newPost)
    {
        var user = await _userManager.FindByIdAsync(uid);

        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        // if (uid != null)
        // {
        //     newPost.Id = uid;
        // }

        
        newPost.Author = user;

        if (ModelState.IsValid == false)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Console.WriteLine(errors);
            return New();
        }


        _context.Posts.Add(newPost);
        await _context.SaveChangesAsync();

        return Dashboard();
    }

    [HttpGet("/lfg/posts/{postId}")]
    public IActionResult ViewPost(int postId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        Post? post = _context.Posts
            .Include(p => p.Author)
            // .Include(p => p.GroupPlayers)
            // .ThenInclude(groupPlayer => groupPlayer.User)
            .FirstOrDefault(p => p.PostId == postId);

        if (post == null)
        {
            return Dashboard();
        }
        return View("ViewPost", post);
    }

    [HttpPost("/lfg/posts/{postId}/delete")]
    public IActionResult Delete(int postId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        Post? postToDelete = _context.Posts.FirstOrDefault(p => p.PostId == postId);

        if (postToDelete != null)
        {
            if (postToDelete.Author.Id == uid)
            {
                _context.Posts.Remove(postToDelete);
                _context.SaveChanges();
            }
        }
        return Dashboard();
    }

    [HttpGet("/lfg/posts/{postId}/edit")]
    public IActionResult Edit(int postId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        Post? post = _context.Posts.FirstOrDefault(p => p.PostId == postId);

        if (post == null || post.Author.Id != uid)
        {
            return Dashboard();
        }
        return View("Edit", post);
    }

    [HttpPost("/lfg/posts/{postId}/update")]
    public IActionResult Update(int postId, Post updatedPost)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        if (ModelState.IsValid == false)
        {
            return Edit(postId);
        }

        Post? dbPost = _context.Posts.FirstOrDefault(p => p.PostId == postId);

        if (dbPost == null || dbPost.Author.Id != uid)
        {
            return Dashboard();
        }

        dbPost.Title = updatedPost.Title;
        dbPost.PlayersOnTeam = updatedPost.PlayersOnTeam;
        dbPost.MaxPlayersOnTeam = updatedPost.MaxPlayersOnTeam;
        // dbPost.PlayersNeeded = updatedPost.PlayersNeeded;
        dbPost.Platform = updatedPost.Platform;
        dbPost.Language = updatedPost.Language;
        dbPost.GroupType = updatedPost.GroupType;
        dbPost.MinLevel = updatedPost.MinLevel;
        dbPost.Description = updatedPost.Description;
        // dbPost.GameId = updatedPost.GameId;
        dbPost.GameActivity = updatedPost.GameActivity;
        dbPost.UpdatedAt = DateTime.Now;
        _context.Posts.Update(dbPost);
        _context.SaveChanges();

        return RedirectToAction("ViewPost", new { postId = dbPost.PostId});
    }

    [HttpPost("/lfg/posts/{postId}/joingroup")]
    public IActionResult JoinGroup(int postId)
    {
        if (!loggedIn || uid == null)
        {
            return RedirectToAction("Index", "User");
        }

        GroupMember? existingMember = _context.GroupMembers.FirstOrDefault(gm => gm.PostId == postId && gm.Id == uid);

        if (existingMember == null)
        {
            GroupMember newMember = new GroupMember(){
                PostId = postId,
                Id = uid
            };
            _context.GroupMembers.Add(newMember);
        }
        else
        {
            _context.Remove(existingMember);
        }

        _context.SaveChanges();
        return Dashboard();
    }

    
    
    
}