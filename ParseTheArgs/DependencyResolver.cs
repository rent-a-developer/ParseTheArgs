using System;

namespace ParseTheArgs
{
    /// <summary>
    /// Resolves dependencies.
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Resolves the dependency of the given type <typeparamref name="TDependency"/> passing along the given constructor arguments.
        /// </summary>
        /// <typeparam name="TDependency">The type of dependency to resolve.</typeparam>
        /// <param name="constructorArguments">The arguments to pass to the constructor of <typeparamref name="TDependency"/>.</param>
        /// <returns>An instance of <typeparamref name="TDependency"/>.</returns>
        TDependency Resolve<TDependency>(params Object[] constructorArguments);
    }

    /// <summary>
    /// The default dependency resolver.
    /// </summary>
    public class DefaultDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Resolves the dependency of the given type <typeparamref name="TDependency"/> passing along the given constructor arguments.
        /// </summary>
        /// <typeparam name="TDependency">The type of dependency to resolve.</typeparam>
        /// <param name="constructorArguments">The arguments to pass to the constructor of <typeparamref name="TDependency"/>.</param>
        /// <returns>An instance of <typeparamref name="TDependency"/>.</returns>
        public virtual TDependency Resolve<TDependency>(params Object[] constructorArguments)
        {
            return (TDependency) Activator.CreateInstance(typeof(TDependency), constructorArguments);
        }
    }

    internal static class Dependencies
    {
        internal static IDependencyResolver Resolver { get; set; } = new DefaultDependencyResolver();
    }
}
