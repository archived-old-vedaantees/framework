using Vedaantees.Framework.Providers.Storages.Keys;
using HashidsNet;

namespace Vedaantees.Framework.Providers.Storages
{
    public class HashKeys : IHashKeys
    {
        private string _salt;

        public HashKeys(string defaultSalt)
        {
            _salt = defaultSalt;
        }
        
        public string Encrypt(long key)
        {
            var hashids = new Hashids(_salt);
            return hashids.EncodeLong(key);
        }

        public long Decrypt(string hash)
        {
            var hashids = new Hashids(_salt);
            return hashids.DecodeLong(hash)[0];
        }

        public void SetSalt(string salt)
        {
            _salt = salt;
        }
    }
}