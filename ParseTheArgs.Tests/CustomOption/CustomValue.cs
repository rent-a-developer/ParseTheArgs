using System;

namespace ParseTheArgs.Tests.CustomOption
{
    public sealed class CustomValue
    {
        public CustomValue(String value)
        {
            this.Value = value;
        }

        public String Value { get; }

        public Boolean Equals(CustomValue other)
        {
            return String.Equals(this.Value, other.Value);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CustomValue customValue && this.Equals(customValue);
        }

        public override Int32 GetHashCode()
        {
            return this.Value?.GetHashCode() ?? 0;
        }
    }
}