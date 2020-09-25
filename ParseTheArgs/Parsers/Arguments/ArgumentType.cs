namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Specifies the type of an argument.
    /// </summary>
    public enum ArgumentType
    {
        /// <summary>
        /// An argument that does not have a value (e.g. a switch argument).
        /// </summary>
        ValuelessArgument,

        /// <summary>
        /// An argument that accepts a single value.
        /// </summary>
        SingleValueArgument,

        /// <summary>
        /// An argument that accepts one or more values.
        /// </summary>
        MultiValueArgument
    }
}