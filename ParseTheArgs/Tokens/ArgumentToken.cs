using System;
using System.Collections.Generic;
using ParseTheArgs.Extensions;

namespace ParseTheArgs.Tokens
{
    /// <summary>
    /// Represents an argument token.
    /// </summary>
    public sealed class ArgumentToken : CommandLineArgumentsToken
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument.</param>
        public ArgumentToken(String argumentName)
        {
            this.ArgumentName = argumentName;
            this.ArgumentValues = new List<String>();
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="argumentValues">The values of the argument.</param>
        public ArgumentToken(String argumentName, List<String> argumentValues) : this(argumentName)
        {
            this.ArgumentValues = argumentValues;
        }

        /// <summary>
        /// The name of the argument.
        /// </summary>
        public String ArgumentName { get; }

        /// <summary>
        /// The values of the argument.
        /// </summary>
        public List<String> ArgumentValues { get; }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((ArgumentToken) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return ((this.ArgumentName?.GetHashCode() ?? 0) * 397) ^ (this.ArgumentValues?.GetHashCode() ?? 0);
            }
        }

        private Boolean Equals(ArgumentToken other)
        {
            if (!String.Equals(this.ArgumentName, other.ArgumentName))
            {
                return false;
            }

            if (this.ArgumentValues == null && other.ArgumentValues == null)
            {
                return true;
            }

            if (this.ArgumentValues == null || other.ArgumentValues == null)
            {
                return false;
            }

            return this.ArgumentValues.HasSameElementsThan(other.ArgumentValues);
        }
    }
}