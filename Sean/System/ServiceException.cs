namespace System
{
    /// <summary>
    /// 此异常用于服务处理中的异常抛出，默认不计入日志
    /// </summary>
    public sealed class ServiceException : Exception
    {
        /// <summary>
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用</param>
        public ServiceException(string message, Exception innerException = null) : base(message, innerException)
        {
            Logging = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="statusCode">异常代码</param>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用</param>
        public ServiceException(int statusCode, string message, Exception innerException = null) : base(message, innerException)
        {
            StatusCode = statusCode;
            Logging = false;
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// 是否错误消息记录日志，默认不写入
        /// </summary>
        public bool Logging { get; set; }
    }
}
