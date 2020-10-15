using FakeItEasy;
using NUnit.Framework;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class BaseTestFixture
    {
        #region Setup/Teardown

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

        #endregion

        protected IDependencyResolver DependencyResolver { get; private set; }
    }
}