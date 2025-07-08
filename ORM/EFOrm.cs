
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.ORM
{
    public class EFOrm(string connection) : IOrm
    {
        public string Connection { get; set; } = connection;
        public void Update<T>(T obj) where T : class
        {
            if (obj == null)
            {
                string error = "this object is null";
                throw new ArgumentNullException(error);
            }
            ApplicationContext Context = new ApplicationContext(Connection);
            Context.Update(obj);
            Context.SaveChanges();
        }

        public void Delete<T>(T obj) where T : class
        {
            if (obj == null)
            {
                string error = "this object is null";
                throw new ArgumentNullException(error);
            }
            ApplicationContext Context = new ApplicationContext(Connection);
            Context.Remove(obj);
            Context.SaveChanges();

        }

        public void Persist<T>(T obj) where T : class
        {
            if (obj == null)
            {
                string error = "this object is null";
                throw new ArgumentNullException(error);
            }
            ApplicationContext Context = new ApplicationContext(Connection);
            Context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users ON");
            Context.Add(obj);
            Context.SaveChanges();
            Context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users OFF");
        }

        public List<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            ApplicationContext Context = new ApplicationContext(Connection);
            return [.. Context.Set<T>().Where(predicate)];
        }
    }
}