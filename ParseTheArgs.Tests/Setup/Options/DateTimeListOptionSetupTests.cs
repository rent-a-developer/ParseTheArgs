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
    public class DateTimeListOptionSetupTests
    {
        [Test(Description = "FormatProvider should apply the specified format to the parser.")]
        public void FormatProvider_CustomFormat_ShouldSetFormatOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<DateTimeListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<DateTimeListOptionParser>(It.Is<PropertyInfo>(p => p.Name == "DateTimes"))).Returns(optionParserMock.Object);
            var setup = new DateTimeListOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.DateTimes));

            var returnedSetup = setup.Format("ddd dd MMM yyyy h:mm tt");

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.DateTimeFormat = It.Is<String>(s => s == "ddd dd MMM yyyy h:mm tt"), Times.Once());
        }

        [Test(Description = "FormatProvider should apply the specified format provider to the parser.")]
        public void FormatProvider_CustomFormatProvider_ShouldSetFormatProviderOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<DateTimeListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<DateTimeListOptionParser>(It.Is<PropertyInfo>(p => p.Name == "DateTimes"))).Returns(optionParserMock.Object);
            var setup = new DateTimeListOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.DateTimes));

            var returnedSetup = setup.FormatProvider(new CultureInfo("en-GB"));

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.FormatProvider = It.Is<CultureInfo>(ci => ci.Name == "en-GB"), Times.Once());
        }

        [Test(Description = "Styles should apply the specified styles to the parser.")]
        public void Styles_CustomStyles_ShouldSetStylesOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<DateTimeListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<DateTimeListOptionParser>(It.Is<PropertyInfo>(p => p.Name == "DateTimes"))).Returns(optionParserMock.Object);
            var setup = new DateTimeListOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.DateTimes));

            var returnedSetup = setup.Styles(DateTimeStyles.AdjustToUniversal);

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.DateTimeStyles = It.Is<DateTimeStyles>(tss => tss == DateTimeStyles.AdjustToUniversal), Times.Once());
        }
    }
}