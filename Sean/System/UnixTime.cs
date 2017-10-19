namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public class UnixTime
    {
        TimeSpan _timeSpan;

        /// <summary>
        /// 默认构造函数，使用当前时间
        /// </summary>
        public UnixTime() : this(DateTime.Now)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        public UnixTime(DateTime dateTime)
        {
            _timeSpan = (dateTime.ToUniversalTime() - UnixEpoch);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dateTime">如果为空，使用当前时间</param>
        public UnixTime(DateTime? dateTime) : this(dateTime == null ? DateTime.Now : dateTime.Value)
        {

        }

        /// <summary>
        /// 返回 1970/1/1 午夜距离该日期时间的秒数。 
        /// </summary>
        public long Seconds
        {
            get { return (long)_timeSpan.TotalSeconds; }
        }

        /// <summary>
        /// 返回 1970/1/1 午夜距离该日期时间的毫秒数。 
        /// </summary>
        public long Milliseconds
        {
            get { return (long)_timeSpan.TotalMilliseconds; }
        }

        public static readonly DateTime UnixEpoch;

        static UnixTime()
        {
            UnixEpoch = new DateTime(1970, 1, 1);
        }

        public static implicit operator UnixTime(DateTime dt)
        {
            return new UnixTime(dt);
        }
    }
}
