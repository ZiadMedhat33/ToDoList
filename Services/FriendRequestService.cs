using ToDoList.ORM;
using ToDoList.Entites.Users;
namespace ToDoList.Services
{
    public class FriendRequestService(IOrm Orm)
    {
        public IOrm ORM { get; set; } = Orm;
        FriendShipService FriendShipService { get; set; } = new FriendShipService(Orm);
        public UserService UserService { get; set; } = new UserService(Orm);
        public void DeleteFriendRequestById(long id)
        {
            ORM.Delete(GetFriendRequestById(id));
        }

        public FriendRequest GetFriendRequestById(long Id)
        {
            if (ORM.Query<FriendRequest>(e => e.Id == Id).Count == 0)
            {
                throw new KeyNotFoundException();
            }
            return ORM.Query<FriendRequest>(e => e.Id == Id)[0];
        }
        public List<FriendRequest> GetFriendRequestOfTheUser(long CallerId)
        {
            return ORM.Query<FriendRequest>(e => e.RecieverId == CallerId);
        }
        public void SendFriendRequest(long senderId, long id) // userId of the reciever
        {
            User sender = UserService.GetUserById(senderId);
            User reciever = UserService.GetUserById(id);
            if (ThereIsAFriendRequest(reciever.Id, sender.Id))
            {
                FriendShipService.AddFriendShip(sender.Id, reciever.Id);
                return;

            }
            else if (FriendShipService.IsFriends(senderId, id))
            {
                throw new InvalidOperationException();
            }
            FriendRequest request = new(sender.Id, reciever.Id);
            ORM.Persist(request);
        }
        public void AcceptFriendRequest(long CallerId, long Id)
        {
            FriendRequest request = GetFriendRequestById(Id);
            if (request.RecieverId != CallerId)
            {
                throw new UnauthorizedAccessException();
            }
            FriendShipService.AddFriendShip(CallerId, Id);
            DeleteFriendRequestById(Id);
        }

        public void DenyFriendRequest(long CallerId, long Id)
        {
            FriendRequest request = GetFriendRequestById(Id);
            if (request.Reciever.Id != CallerId)
            {
                throw new UnauthorizedAccessException();
            }
            DeleteFriendRequestById(Id);
        }
        public bool ThereIsAFriendRequest(long Id1, long Id2)
        {
            long request = ORM.Query<FriendRequest>(e => e.Sender.Id == Id1 && e.Reciever.Id == Id2).Count;
            return request != 0;
        }


    }
}