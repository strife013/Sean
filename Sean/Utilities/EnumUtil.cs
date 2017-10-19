using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DisplayNameAttribute = Sean.ComponentModel.DisplayNameAttribute;

namespace Sean.Utilities
{
    public static class EnumUtil
    {
        #region --- 枚举描述元数据 ---

        /// <summary>
        /// 根据枚举字段上的System.ComponentModel.DescriptionAttribute属性获取对应的描述
        /// </summary>
        /// <returns>返回显示名称</returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var field = type.GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute != null ? attribute.Description : String.Empty;
        }

        /// <summary>
        /// 根据枚举字段上的System.ComponentModel.DescriptionAttribute属性获取对应的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回字段名+描述的键值集合</returns>
        public static Dictionary<string, string> GetDescriptionMeta<T>() where T : struct,IConvertible
        {
            var type = typeof(T);

            if (!type.IsEnum) throw new ArgumentException("T is not a enum type");

            var dic = new Dictionary<string, string>();

            foreach (var field in type.GetFields())
            {
                if (field.FieldType.IsEnum)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    dic[field.Name] = attribute != null ? attribute.Description : String.Empty;
                }
            }

            return dic;
        }

        /// <summary>
        /// 根据枚举字段上的System.ComponentModel.DescriptionAttribute属性获取对应的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回枚举值+描述的键值集合</returns>
        public static Dictionary<T, string> GetDescriptionMetaEx<T>() where T : struct,IConvertible
        {
            var type = typeof(T);

            if (!type.IsEnum) throw new ArgumentException("T is not a enum type");

            var dic = new Dictionary<T, string>();

            foreach (var field in type.GetFields())
            {
                if (field.FieldType.IsEnum)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    var enumValue = (T)field.GetRawConstantValue();
                    dic[enumValue] = attribute != null ? attribute.Description : String.Empty;
                }
            }

            return dic;
        }

        #endregion

        #region --- 枚举显示元数据 ---

        /// <summary>
        /// 根据枚举字段上的Sean.ComponentModel.DisplayNameAttribute属性获取对应的描述
        /// </summary>
        /// <returns>返回显示名称</returns>
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            var field = type.GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
            return attribute != null ? attribute.DisplayName : String.Empty;
        }

        /// <summary>
        /// 根据枚举字段上的Sean.ComponentModel.DisplayNameAttribute属性获取对应的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回字段名+描述的键值集合</returns>
        public static Dictionary<string, string> GetDisplayMeta<T>() where T : struct,IConvertible
        {
            var type = typeof(T);

            if (!type.IsEnum) throw new ArgumentException("T is not a enum type");

            var dic = new Dictionary<string, string>();

            foreach (var field in type.GetFields())
            {
                if (field.FieldType.IsEnum)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                    dic[field.Name] = attribute != null ? attribute.DisplayName : String.Empty;
                }
            }

            return dic;
        }

        /// <summary>
        /// 根据枚举字段上的Sean.ComponentModel.DisplayNameAttribute属性获取对应的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回枚举值+描述的键值集合</returns>
        public static Dictionary<T, string> GetDisplayMetaEx<T>() where T : struct,IConvertible
        {
            var type = typeof(T);

            if (!type.IsEnum) throw new ArgumentException("T is not a enum type");

            var dic = new Dictionary<T, string>();

            foreach (var field in type.GetFields())
            {
                if (field.FieldType.IsEnum)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                    var enumValue = (T)field.GetRawConstantValue();
                    dic[enumValue] = attribute != null ? attribute.DisplayName : String.Empty;
                }
            }

            return dic;
        }

        #endregion

        #region ---- 检查枚举定义是否有重复 ----

        /// <summary>
        /// 检查枚举定义是否有重复,返回null表示没有重复
        /// </summary>
        public static string CheckRepeate<T>()
        {
            var type = typeof(T);

            if (!type.IsEnum) throw new ArgumentException("T is not a enum type");

            var hashSet = new HashSet<int>();
            var fields = type.GetFields();
            var errorFields = new List<string>();

            foreach (var field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    var intValue = (int)field.GetRawConstantValue();
                    if (hashSet.Contains(intValue))
                    {
                        errorFields.Add(field.Name);
                    }
                    else hashSet.Add(intValue);
                }
            }
            return errorFields.Any() ? String.Join(",", errorFields) : null;
        }

        #endregion

        /// <summary>
        /// 获取所有枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<T> All<T>()
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new ArgumentException("not enum type");

            foreach (var field in type.GetFields())
            {
                if (field.FieldType.IsEnum)
                {
                    yield return (T)field.GetRawConstantValue();
                }
            }
        }
    }
}
