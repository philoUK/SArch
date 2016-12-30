using System;
using Domain;

namespace Persistence
{
    public interface IRepository<T> where T: AggregateRoot
    {
        T Load(Guid id);
        void Save(T root);
    }
}