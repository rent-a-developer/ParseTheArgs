using System;

namespace ParseTheArgs.Tokens
{
    /// <summary>
    /// Represents a command token.
    /// </summary>
    public sealed class CommandToken : CommandLineArgumentsToken
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        public CommandToken(String commandName)
        {
            this.CommandName = commandName;
        }

        /// <summary>
        /// The name of the command.
        /// </summary>
        public String CommandName { get; }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((CommandToken) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode()
        {
            return this.CommandName?.GetHashCode() ?? 0;
        }

        private Boolean Equals(CommandToken other)
        {
            return String.Equals(this.CommandName, other.CommandName);
        }
    }
}