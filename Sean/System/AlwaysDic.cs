using System.Collections.Generic;

namespace System
{
    public class AlwaysDic<TKey, TValue>
    {
        Dictionary<TKey, TValue> _dic;

        public AlwaysDic(Dictionary<TKey, TValue> dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException(nameof(dic));
            }
            _dic = dic;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue val;
                return _dic.TryGetValue(key, out val) ? val : default(TValue);
            }
            set
            {
                _dic[key] = value;
            }
        }
    }
}
