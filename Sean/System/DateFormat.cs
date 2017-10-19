namespace System
{
    /// <summary>
    /// DateTime Format
    /// </summary>
    public enum DateFormat
    {
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        dash_y4MdHms,

        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        dash_y4MdHm,

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        dash_y4Md,

        /// <summary>
        /// yyyy/MM/dd HH:mm:ss
        /// </summary>
        slash_y4MdHms,

        /// <summary>
        /// yyyy/MM/dd HH:mm
        /// </summary>
        slash_y4MdHm,

        /// <summary>
        /// yyyy/MM/dd
        /// </summary>
        slash_y4Md,

        /// <summary>
        /// yyyy年MM月dd日 HH:mm
        /// </summary>
        china_y4MdHm,

        /// <summary>
        /// yyyy年MM月dd日
        /// </summary>
        china_y4Md,

        /// <summary>
        /// yy年MM月dd日 HH:mm
        /// </summary>
        china_y2MdHm,

        /// <summary>
        /// MM月dd日 HH:mm
        /// </summary>
        china_MdHm
    }
}
