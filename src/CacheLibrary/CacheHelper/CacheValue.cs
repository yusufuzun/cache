namespace CacheLibrary.CacheHelper
{
    /// <summary>
    /// Wrapped Cache object of binary sequence
    /// </summary>
    public class CacheValue
    {
        public CacheValue(byte[] value)
        {
            Value = value;
        }

        public byte[] Value { get; private set; }
    }
}