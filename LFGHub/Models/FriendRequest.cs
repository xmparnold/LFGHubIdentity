#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LFGHub.Models;

public class FriendRequest
{
    [Key]
    public string FriendRequestId { get; set; }
    public bool Approved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string Id1 { get; set; }
    public AdvanceUser Requestor { get; set; }

    public string Id2 { get; set; }
    public AdvanceUser Reciever { get; set; }

}
