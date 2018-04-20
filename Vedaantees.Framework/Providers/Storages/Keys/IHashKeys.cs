#region  usings 

#endregion

namespace Vedaantees.Framework.Providers.Storages.Keys
{
    public interface IHashKeys
    {
        string Encrypt(long key);
        long Decrypt(string hash);
        void SetSalt(string salt);
    }
}