using System;

namespace ParseTheArgs
{
    /// <summary>
    /// Represents the name (the name and optionally the short name) of an argument.
    /// </summary>
    public sealed class ArgumentName
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ArgumentName()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        public ArgumentName(String name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="shortName">The short name of the argument.</param>
        public ArgumentName(String name, Char shortName)
        {
            this.Name = name;
            this.ShortName = shortName;
        }

        /// <summary>
        /// The name of the argument.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The short name of the argument.
        /// The value is null if the argument does not have a short name.
        /// </summary>
        public Nullable<Char> ShortName { get; set; }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override Boolean Equals(Object obj)
        {
            return ReferenceEquals(this, obj) || obj is ArgumentName other && this.Equals(other);
        }

        /// <summary>
        /// Determines if the given string is equal to the name or short name of this instance.
        /// </summary>
        /// <param name="nameOrShortName">A name or short name of an argument.</param>
        /// <returns>True if the given string is equal the name or short name of this instance; otherwise, false.</returns>
        public Boolean EqualsNameOrShortName(String nameOrShortName)
        {
            return this.Name == nameOrShortName || (this.ShortName != null && this.ShortName.Value.ToString() == nameOrShortName);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return ((this.Name != null ? this.Name.GetHashCode() : 0) * 397) ^ this.ShortName.GetHashCode();
            }
        }

        /// <summary>
        /// Returns a textual representation of this instance.
        /// If this instance has a short name "-shortName (--name)" is returned, otherwise "--name" is returned.
        /// </summary>
        /// <returns>A textual representation of this instance.</returns>
        public override String ToString()
        {
            if (this.ShortName != null)
            {
                return $"-{this.ShortName} (--{this.Name})";
            }
            else
            {
                return $"--{this.Name}";
            }
        }

        private Boolean Equals(ArgumentName other)
        {
            return this.Name == other.Name && this.ShortName == other.ShortName;
        }
    }
}