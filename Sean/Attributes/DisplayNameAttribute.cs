using System;

namespace Sean.ComponentModel
{
    public sealed class DisplayNameAttribute : Attribute
    {
        public static readonly DisplayNameAttribute Default = new DisplayNameAttribute();

        public string DisplayName
        {
            get
            {
                return DisplayNameValue;
            }
        }

        private string DisplayNameValue { get; set; }

        public DisplayNameAttribute()
            : this(string.Empty)
        {
        }

        public DisplayNameAttribute(string displayName)
        {
            DisplayNameValue = displayName;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            var displayNameAttribute = obj as DisplayNameAttribute;
            return displayNameAttribute != null && displayNameAttribute.DisplayName == DisplayName;
        }

        public override int GetHashCode()
        {
            return DisplayName.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return Equals(Default);
        }
    }
}
