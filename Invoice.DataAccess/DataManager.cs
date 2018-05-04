using NHibernate;
using System.Collections.Generic;

namespace Invoice.DataAccess
{
    public class DataManager
    {
        public IList<T> RetrieveData<T>(ISession session) where T : class
        {
            return session.CreateCriteria<T>().List<T>();
        }

        public object CreateData<T>(ISession session, T model) where T : class
        {
            return session.Save(model);
        }
    }
}
