using System.Linq.Expressions;

namespace ToDoList.ORM
{
    public interface IOrm
    {
        public void Update<T>(T obj) where T : class;
        public void Delete<T>(T obj) where T : class;
        public void Persist<T>(T obj) where T : class;
        public List<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class;

    }
}