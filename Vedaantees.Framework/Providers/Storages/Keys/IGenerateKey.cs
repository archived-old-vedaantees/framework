namespace Vedaantees.Framework.Providers.Storages.Keys
{
    //TODO: Make this obsolete. Use IDocumentStore instead.
    public interface IGenerateKey
    {
        long GetNextNumericalKey(string collectionName);

        string GetNextStringKey(string collectionName);
    }
}