namespace System
{
    public static class ObjectExtension
    {
        /// <summary>
        /// object.ToString扩展,支持obj为null时调用
        /// </summary>
        public static string ToString(this object obj, bool lower)
        {
            return obj == null ? null : lower ? obj.ToString().ToLowerInvariant() : obj.ToString();
        }
    }
}
