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
    public class GuidListOptionSetupTests
    {
        [Test(Description = "Format should apply the specified format to the parser.")]
        public void Format_CustomFormat_ShouldSetFormatOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<GuidListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<GuidListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("Guids"))).Returns(optionParser);
            var setup = new GuidListOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.Guids));

            var returnedSetup = setup.Format("N");

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.GuidFormat).To("N").MustHaveHappened();
        }
    }
}