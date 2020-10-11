using FakeItEasy;
using NUnit.Framework;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class BaseTestFixture
    {
        protected IDependencyResolver DependencyResolver { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.DependencyResolver = A.Fake<DefaultDependencyResolver>(ob => ob.CallsBaseMethods());
            Dependencies.Resolver = this.DependencyResolver;
        }

        [TearDown]
        public void TearDown()
        {
            Dependencies.Resolver = new DefaultDependencyResolver();
        }
    }
}
