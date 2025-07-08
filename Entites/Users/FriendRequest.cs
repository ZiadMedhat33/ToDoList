using System.ComponentModel.DataAnnotations;

namespace ToDoList.Entites.Users
{
    public class FriendRequest
    {
        public FriendRequest(long senderId, long recieverId)
        {
            SenderId = senderId;
            RecieverId = recieverId;
        }
        protected FriendRequest() { }
        [Key]
        public long Id { get; set; }

        [Required]
        public User Sender { get; set; }
        [Required]
        public User Reciever { get; set; }

        public long SenderId { get; set; }
        public long RecieverId { get; set; }

        public long TimeStamp { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}