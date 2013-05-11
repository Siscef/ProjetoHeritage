using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heritage.Models.ContextoBanco
{
    public interface IContextoDados : IDisposable
    {
        IQueryable<T> GetAll<T>() where T : class;
        T Get<T>(long id) where T : class;
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;

        void Setup();
        void SaveChanges();


    }
}
