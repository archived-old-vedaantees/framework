namespace Vedaantees.Framework.Providers.Storages.Keys
{
    public interface IGenerateKey
    {
        long GetNextNumericalKey(string collectionName);

        string GetNextStringKey(string collectionName);
    }
}