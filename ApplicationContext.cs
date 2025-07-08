using Microsoft.EntityFrameworkCore;
using ToDoList.Entites.Users;
using ToDoList.Entites;
namespace ToDoList
{
    public class ApplicationContext(string connection) : DbContext
    {
        public string Connection { get; set; } = connection;
        public DbSet<User> Users { get; set; }
        public DbSet<Work> Tasks { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(e => e.Works).WithOne(e => e.User);
            modelBuilder.Entity<User>().HasIndex(f => f.UserName).IsUnique();
            modelBuilder.Entity<User>().HasMany(e => e.SentRequests).WithOne(e => e.Sender).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasMany(e => e.RecievedRequests).WithOne(e => e.Reciever).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasMany(u => u.Friends).WithOne(f => f.Friend).HasForeignKey(f => f.FriendId).
            OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasMany(u => u.FriendOf).WithOne(f => f.FriendOf).HasForeignKey(f => f.FriendOfId).
            OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FriendShip>().HasKey(f => new { f.FriendId, f.FriendOfId });
            modelBuilder.Entity<FriendRequest>().HasIndex(f => new { f.SenderId, f.RecieverId }).IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Connection);

        }

    }
}