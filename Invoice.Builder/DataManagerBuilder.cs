using Invoice.DataAccess;

namespace Invoice.Builder
{
    public class DataManagerBuilder
    {
        public void BuildSessionFactory(string configurationFilePath)
        {
            NHibernateHelper.SetConfigurationFilePath(configurationFilePath);
        }
    }
}
