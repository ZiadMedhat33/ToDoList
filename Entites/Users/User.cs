using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ToDoList.Entites.Users
{
    public class User
    {

        public User(string userName, string password, UserType Type)
        {
            UserName = userName;
            Password = password;
            UserType = Type;
        }
        protected User() { }
        [Key]
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public UserType UserType { get; set; }

        public List<Work> Works { get; set; } = [];

        public List<FriendShip> Friends { get; set; } = [];

        public List<FriendShip> FriendOf { get; set; } = [];

        public List<FriendRequest> SentRequests { get; set; } = [];

        public List<FriendRequest> RecievedRequests { get; set; } = [];
        public int WorkCount { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
    }
    public enum UserType
    {
        Normal = 0,
        Premium = 1,
        Admin = 2
    }

}
