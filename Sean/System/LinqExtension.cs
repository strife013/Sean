using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtension
    {
        public static AlwaysDic<TKey, TValue> ToAlwaysDic<TKey, TValue>(this Dictionary<TKey, TValue> dic)
        {
            return new AlwaysDic<TKey, TValue>(dic);
        }

        /// <summary>
        /// Checks whatever given collection object is null or has no item.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultVal = default(TValue))
        {
            TValue val;
            return dic.TryGetValue(key, out val) ? val : defaultVal;
        }
    }
}
