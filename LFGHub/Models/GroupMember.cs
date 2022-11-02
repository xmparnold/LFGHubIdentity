#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LFGHub.Models;

public class GroupMember
{
    [Key]
    public string GroupMemberId { get; set; }
    public int PostId { get; set; }
    public Post? Post { get; set; }
    public string Id { get; set; }
    public AdvanceUser? User { get; set; }
}