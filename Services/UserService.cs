using ToDoList.Entites.Users;
using ToDoList.ORM;
using ToDoList.Entites;
namespace ToDoList.Services
{
    public class UserService(IOrm Orm)
    {
        public IOrm ORM { get; set; } = Orm;

        public void DeleteUserById(long Id)
        {

            User User = GetUserById(Id);
            User.UserName = null;
            User.Password = null;
            ORM.Update(User);
            List<Work> userWorks = ORM.Query<Work>(e => e.UserId == Id);
            foreach (Work work in userWorks)
            {
                ORM.Delete(work);
            }

        }

        public User GetUserById(long Id)
        {
            if (ORM.Query<User>(e => e.Id == Id).Count == 0)
            {
                throw new KeyNotFoundException();
            }
            return ORM.Query<User>(e => e.Id == Id)[0];
        }
        public User Login(string UserName, string password)
        {
            if (UserName == null || password == null)
            {
                throw new ArgumentNullException();
            }
            if (ORM.Query<User>(e => e.UserName == UserName && e.Password == password).Count == 0)
            {
                throw new KeyNotFoundException();
            }
            return ORM.Query<User>(e => e.UserName == UserName && e.Password == password)[0];
        }

        public void Register(string username, string password, UserType Type)
        {
            if (username == null || password == null)
            {
                throw new ArgumentNullException();
            }
            User User = new(username, password, Type);
            ORM.Persist(User);
        }
        public void UpdateUser(long CallerId, User user)
        {
            User Caller = GetUserById(CallerId);
            User OldUser = GetUserById(user.Id); // to throw an exception if the user does not exist.
            if (Caller.UserType != UserType.Admin && CallerId != user.Id)
            {
                throw new UnauthorizedAccessException();
            }
            User NewUser = new(user.UserName, user.Password, user.UserType);
            NewUser.Id = user.Id;
            ORM.Update(user); // will throw an exception if user is null
        }

        /////////Works///////////
    }
}