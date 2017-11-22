using System;
using System.Collections.Generic;

namespace SampleBot.Shared.Database
{
    interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T o);
        void Update(T o);
        void Delete(int id);
        void Save();
    }
}
