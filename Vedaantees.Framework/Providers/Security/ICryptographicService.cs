namespace Vedaantees.Framework.Providers.Security
{
    public interface ICryptographicService
    {
        string ComputeHash(string stringToHash);
    }
}