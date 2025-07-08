using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Entites.Users
{

    public class FriendShip
    {
        public FriendShip(long friendId, long friendOfId)
        {
            FriendId = friendId;
            FriendOfId = friendOfId;
        }
        public FriendShip() { }
        public User Friend { get; set; }
        public User FriendOf { get; set; }
        [Required]
        public long FriendId { get; set; }
        [Required]
        public long FriendOfId { get; set; }
    }
}