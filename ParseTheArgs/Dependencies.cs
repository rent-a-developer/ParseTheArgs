namespace ParseTheArgs
{
    internal static class Dependencies
    {
        internal static IDependencyResolver Resolver { get; set; } = new DefaultDependencyResolver();
    }
}
