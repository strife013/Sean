using System.Globalization;

namespace System
{
    /// <summary>
    /// datetime extension
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// format datetime
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToFormat(this DateTime dt, DateFormat format)
        {
            switch (format)
            {
                case DateFormat.dash_y4Md:
                    return dt.ToString(DateFormatConst.dash_y4Md);
                case DateFormat.dash_y4MdHm:
                    return dt.ToString(DateFormatConst.dash_y4MdHm);
                case DateFormat.dash_y4MdHms:
                    return dt.ToString(DateFormatConst.dash_y4MdHms);

                case DateFormat.slash_y4Md:
                    return dt.ToString(DateFormatConst.slash_y4Md);
                case DateFormat.slash_y4MdHm:
                    return dt.ToString(DateFormatConst.slash_y4MdHm);
                case DateFormat.slash_y4MdHms:
                    return dt.ToString(DateFormatConst.slash_y4MdHms);

                case DateFormat.china_MdHm:
                    return dt.ToString(DateFormatConst.china_MdHm);
                case DateFormat.china_y4Md:
                    return dt.ToString(DateFormatConst.china_y4Md);
                case DateFormat.china_y2MdHm:
                    return dt.ToString(DateFormatConst.china_y2MdHm);
                case DateFormat.china_y4MdHm:
                    return dt.ToString(DateFormatConst.china_y4MdHm);

                default:
                    return dt.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToFormat(this DateTime? dt, DateFormat format)
        {
            return dt == null ? string.Empty : ToFormat(dt.Value, format);
        }

        /// <summary>
        /// 转化为中文的周几
        /// </summary>
        public static string ToChinaWeek(this DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "周一";
                case DayOfWeek.Tuesday:
                    return "周二";
                case DayOfWeek.Wednesday:
                    return "周三";
                case DayOfWeek.Thursday:
                    return "周四";
                case DayOfWeek.Friday:
                    return "周五";
                case DayOfWeek.Saturday:
                    return "周六";
                case DayOfWeek.Sunday:
                    return "周日";
                default:
                    return string.Empty;
            }
        }

        public static string ToString(this DateTime? dateTime,string format)
        {
            return dateTime?.ToString(format) ?? string.Empty;
        }
    }
}
