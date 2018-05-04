using Invoice.Model;
using NHibernate;
using NHibernate.Cfg;

namespace Invoice.DataAccess
{
    public class NHibernateHelper
    {
        private static string _configurationFilePath;
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure(_configurationFilePath);
                    configuration.AddAssembly(typeof(TransactionModel).Assembly);

                    _sessionFactory = configuration.BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static void SetConfigurationFilePath(string configurationFilePath)
        {
            _configurationFilePath = configurationFilePath;
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
