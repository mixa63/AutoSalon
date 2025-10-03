using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBBroker
{
    /// <summary>
    /// Interfejs za broker klase koji omogućava unit testiranje i decoupling od konkretne implementacije.
    /// </summary>
    public interface IBroker
    {
        void OpenConnection();
        void CloseConnection();
        void BeginTransaction();
        void Commit();
        void Rollback();

        void Add(IEntity entity);
        int AddWithReturnId(IEntity entity);
        void Update(IEntity entity);
        void Delete(IEntity entity);
        List<IEntity> GetAll(IEntity entity);
        List<IEntity> GetByCondition(IEntity entity);
        int GetFirstId(IEntity entity);
    }
}
