namespace CacheLibrary.CacheHelper
{
    /// <summary>
    /// CacheKey object for key definitions
    /// </summary>
    public class CacheKey
    {
        private readonly string key;

        public CacheKey(string key)
        {
            this.key = key;
        }

        public static implicit operator string(CacheKey key)
        {
            return key.ToString();
        }

        public static implicit operator CacheKey(string key)
        {
            return new CacheKey(key);
        }

        public override string ToString()
        {
            return key;
        }
    }
}