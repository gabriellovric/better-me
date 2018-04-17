using System.Collections.Generic;

namespace BetterMeApi.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> All { get; }

        bool DoesItemExist(long id);

        T Find(long id);

        void Insert(T item);

        void Update(T item);

        void Delete(long id);
    }
}
