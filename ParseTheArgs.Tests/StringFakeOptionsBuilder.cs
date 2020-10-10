using System;
using FakeItEasy;

namespace ParseTheArgs.Tests
{
    public class StringDummyFactory : DummyFactory<String>
    {
        protected override String Create()
        {
            return "Fake String";
        }
    }
}
