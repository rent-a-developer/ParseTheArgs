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
        public ArgumentName(String name, Char? shortName)
        {
            this.Name = name;
            this.ShortName = shortName;
        }

        /// <summary>
        /// The name of the argument.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// The short name of the argument.
        /// The value is null if the argument does not have a short name.
        /// </summary>
        public Char? ShortName { get; }

        /// <summary>
        /// Determines if the given string is equal to the name or short name of this instance.
        /// </summary>
        /// <param name="nameOrShortName">A name or short name of an argument.</param>
        /// <returns>True if the given string is equal the name or short name of this instance; otherwise, false.</returns>
        public Boolean EqualsNameOrShortName(String nameOrShortName)
        {
            return this.Name == nameOrShortName || (this.ShortName != null && this.ShortName.Value.ToString() == nameOrShortName);
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
    }
}