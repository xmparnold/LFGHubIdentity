#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LFGHub.Models;

public class Friend
{
    [Key]
    public string FriendId { get; set; }
    public string Id1 { get; set; }
    public AdvanceUser? User1 { get; set; }
    public string Id2 { get; set; }
    public AdvanceUser? User2 { get; set; }
}