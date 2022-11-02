using Microsoft.AspNetCore.Identity;
using LFGHub.Models;

public class AdvanceUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public string MobileNumber { get; set; }

    public string DestinyUsername { get; set; }

    public List<GroupMember> GroupsIn { get; set; } = new List<GroupMember>();
    public virtual ICollection<Post> Posts { get; set; }
}