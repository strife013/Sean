using System;

namespace Sean.Utilities
{
    public static class TryCatch
    {
        public static void Do(Action action, Action<Exception> errorHandler = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);
            }
        }

        public static void Do<TParam>(Action<TParam> action, TParam parameter, Action<Exception> errorHandler = null)
        {
            try
            {
                action(parameter);
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);
            }
        }

        public static TResult Do<TResult>(Func<TResult> func, Action<Exception> errorHandler = null)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);
            }
            return default(TResult);
        }

        public static TResult Do<TParam, TResult>(Func<TParam, TResult> func, TParam parameter, Action<Exception> errorHandler = null)
        {
            try
            {
                return func(parameter);
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);
            }
            return default(TResult);
        }
    }
}
