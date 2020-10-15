namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Specifies the type of an option.
    /// </summary>
    public enum OptionType
    {
        /// <summary>
        /// An option that does not have a value (e.g. a switch option).
        /// </summary>
        ValuelessOption,

        /// <summary>
        /// An option that accepts a single value.
        /// </summary>
        SingleValueOption,

        /// <summary>
        /// An option that accepts one or more values.
        /// </summary>
        MultiValueOption
    }
}