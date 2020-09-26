using System;
using System.Collections.Generic;

namespace ParseTheArgs.Tokens
{
    /// <summary>
    /// A token that represents a command line argument.
    /// </summary>
    public sealed class ArgumentToken : Token
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
    }
}