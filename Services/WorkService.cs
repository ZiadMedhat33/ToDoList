using ToDoList.ORM;
using ToDoList.Entites;
using ToDoList.Entites.Users;
namespace ToDoList.Services
{
    public class WorkService(IOrm Orm)
    {
        public IOrm ORM { get; set; } = Orm;
        public UserService UserService { get; set; } = new(Orm);

        private Work GetWorkById(long Id)
        {
            if (ORM.Query<Work>(e => e.Id == Id).Count == 0)
            {
                throw new KeyNotFoundException();
            }
            return ORM.Query<Work>(e => e.Id == Id)[0];
        }
        private void DeleteWorkById(long Id)
        {
            ORM.Delete(GetWorkById(Id));
        }
        public List<Work> GetUserWorks(long CallerId, long id)
        {
            List<Work> Works = ORM.Query<Work>(e => e.UserId == id);
            List<Work> AllowedWorks = [];
            foreach (Work Work in Works)
            {
                if (id == CallerId || !Work.ItsPrivate)
                {
                    AllowedWorks.Add(Work);
                }
            }
            return AllowedWorks;
        }
        public void DeleteWork(long CallerId, long Id)
        {
            Work Work = GetWorkById(Id);
            User User = UserService.GetUserById(CallerId);
            if (Work.UserId != User.Id)
            {
                throw new UnauthorizedAccessException();
            }
            DeleteWorkById(Id);
        }
        public void AddWork(long CallerId, string content, bool ItsPriv)
        {
            Work Work = new(CallerId, content, ItsPriv);
            User User = UserService.GetUserById(CallerId);
            if (User.WorkCount < 10 || User.UserType == UserType.Premium || User.UserType == UserType.Admin)
            {
                ORM.Persist(Work);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
            User.WorkCount++;
            ORM.Update(User);
        }
        public void Done(long CallerId, long workId)
        {
            Work work = GetWorkById(workId);
            if (work.UserId != CallerId)
            {
                throw new UnauthorizedAccessException();
            }
            work.Done = true;
            ORM.Update(work);
        }

    }
}