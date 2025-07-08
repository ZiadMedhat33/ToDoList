using ToDoList.ORM;
using ToDoList.Entites.Users;
namespace ToDoList.Services
{
    public class FriendShipService(IOrm Orm)
    {
        public IOrm ORM { get; set; } = Orm;
        public UserService UserService { get; set; } = new UserService(Orm);
        public FriendShip GetFriendShipById(long FriendId, long FriendOfId)
        {
            if (ORM.Query<FriendShip>(e => e.FriendId == FriendId && e.FriendOfId == FriendOfId).Count == 0)
            {
                throw new KeyNotFoundException();
            }
            return ORM.Query<FriendShip>(e => e.FriendId == FriendId && e.FriendOfId == FriendOfId)[0];
        }
        private void DeleteFriendShipById(long FriendId, long FriendOfId)
        {
            ORM.Delete(GetFriendShipById(FriendId, FriendOfId));
        }
        public void AddFriendShip(long Friend1Id, long Friend2Id)
        {
            FriendShip FriendShip = new(Friend1Id, Friend2Id);
            FriendShip ReversefriendShip = new(Friend2Id, Friend1Id);
            ORM.Persist(FriendShip);
            ORM.Persist(ReversefriendShip);
        }
        public void UnFriend(long CallerId, long Id)
        {
            User User = UserService.GetUserById(CallerId);
            User NonFriend = UserService.GetUserById(Id);
            DeleteFriendShipById(User.Id, NonFriend.Id);
            DeleteFriendShipById(NonFriend.Id, User.Id);

        }
        public bool IsFriends(long Id1, long Id2)
        {
            List<FriendShip> relation1 = ORM.Query<FriendShip>(e => e.FriendId == Id1 && e.FriendOfId == Id2);
            return relation1.Count != 0;
        }
        public List<User> SeeUserFriends(long Id)
        {
            return ORM.Query<User>(e => e.Friends.Any(f => f.FriendOfId == Id));
        }

    }
}