using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ToDoList.Entites.Users;
namespace ToDoList.Entites
{
    public class Work
    {
        public Work(long userId, string content, bool itsPriv)
        {
            UserId = userId;
            Content = content;
            ItsPrivate = itsPriv;
        }
        public Work() { }
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public string Content { get; set; }
        public bool Done { get; set; } = false;
        public User User { get; set; }
        public bool ItsPrivate { get; set; }
    }
}