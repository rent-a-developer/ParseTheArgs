using System;
using System.Globalization;
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
    public class DateTimeOptionSetupTests
    {
        [Test(Description = "FormatProvider should apply the specified format to the parser.")]
        public void FormatProvider_CustomFormat_ShouldSetFormatOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<DateTimeOptionParser>(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<DateTimeOptionParser>(It.Is<PropertyInfo>(p => p.Name == "DateTime"))).Returns(optionParserMock.Object);
            var setup = new DateTimeOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.DateTime));

            var returnedSetup = setup.Format("ddd dd MMM yyyy h:mm tt");

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.DateTimeFormat = It.Is<String>(s => s == "ddd dd MMM yyyy h:mm tt"), Times.Once());
        }

        [Test(Description = "FormatProvider should apply the specified format provider to the parser.")]
        public void FormatProvider_CustomFormatProvider_ShouldSetFormatProviderOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<DateTimeOptionParser>(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<DateTimeOptionParser>(It.Is<PropertyInfo>(p => p.Name == "DateTime"))).Returns(optionParserMock.Object);
            var setup = new DateTimeOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.DateTime));

            var returnedSetup = setup.FormatProvider(new CultureInfo("en-GB"));

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.FormatProvider = It.Is<CultureInfo>(ci => ci.Name == "en-GB"), Times.Once());
        }

        [Test(Description = "Styles should apply the specified styles to the parser.")]
        public void Styles_CustomStyles_ShouldSetStylesOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<DateTimeOptionParser>(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<DateTimeOptionParser>(It.Is<PropertyInfo>(p => p.Name == "DateTime"))).Returns(optionParserMock.Object);
            var setup = new DateTimeOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.DateTime));

            var returnedSetup = setup.Styles(DateTimeStyles.AdjustToUniversal);

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.DateTimeStyles = It.Is<DateTimeStyles>(tss => tss == DateTimeStyles.AdjustToUniversal), Times.Once());
        }
    }
}