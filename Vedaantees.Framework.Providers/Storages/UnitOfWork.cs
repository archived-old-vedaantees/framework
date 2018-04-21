using Vedaantees.Framework.Providers.Storages.Sessions;

namespace Vedaantees.Framework.Providers.Storages
{
    public class UnitOfWork
    {
        public UnitOfWork(DocumentSessionFactory documentSessionFactory, SqlStore sqlStore, GraphStore graphStore)
        {
            DocumentSessionFactory = documentSessionFactory;
            SqlStore = sqlStore;
            GraphStore = graphStore;
        }

        public DocumentSessionFactory DocumentSessionFactory { get; private set; }
        public SqlStore SqlStore { get; private set; }
        public GraphStore GraphStore { get; private set; }
    }
}