using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Moq;
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
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<GuidOptionParser>(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<GuidOptionParser>(It.Is<PropertyInfo>(p => p.Name == "Guid"))).Returns(optionParserMock.Object);
            var setup = new GuidOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.Guid));

            var returnedSetup = setup.Format("N");

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.GuidFormat = It.Is<String>(s => s == "N"), Times.Once());
        }
    }
}
