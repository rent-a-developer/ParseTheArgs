using System;
using System.Linq.Expressions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Setup.Options;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests.Setup.Options
{
    [TestFixture]
    public class GuidOptionSetupTests
    {
        [Test(Description = "Format should apply the specified format to the parser.")]
        public void Format_CustomFormat_ShouldSetFormatOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<GuidOptionParser>(ob => ob.WithArgumentsForConstructor(() => new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<GuidOptionParser>(typeof(DataTypesCommandOptions).GetProperty("Guid"))).Returns(optionParser);
            var setup = new GuidOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.Guid));

            var returnedSetup = setup.Format("N");

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.GuidFormat).To("N").MustHaveHappened();
        }
    }
}
