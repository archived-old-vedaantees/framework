using Vedaantees.Framework.Providers.Storages.Sessions;

namespace Vedaantees.Framework.Providers.Storages
{
    public class UnitOfWork
    {
        public void SetStore(DocumentSessionFactory documentSessionFactory)
        {
            DocumentSessionFactory = documentSessionFactory;
        }

        public void SetStore(SqlStore sqlStore)
        {
            SqlStore = sqlStore;
        }

        public void SetStore(GraphStore graphStore)
        {
            GraphStore = graphStore;
        }

        public DocumentSessionFactory DocumentSessionFactory { get; private set; }
        public SqlStore SqlStore { get; private set; }
        public GraphStore GraphStore { get; private set; }
    }
}