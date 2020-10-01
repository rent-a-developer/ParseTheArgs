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
    public class DecimalOptionSetupTests
    {
        [Test(Description = "FormatProvider should apply the specified format provider to the parser.")]
        public void FormatProvider_CustomFormatProvider_ShouldSetFormatProviderOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<DecimalOptionParser>(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<DecimalOptionParser>(It.Is<PropertyInfo>(p => p.Name == "Decimal"))).Returns(optionParserMock.Object);
            var setup = new DecimalOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.Decimal));

            var returnedSetup = setup.FormatProvider(new CultureInfo("en-GB"));

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.FormatProvider = It.Is<CultureInfo>(ci => ci.Name == "en-GB"), Times.Once());
        }

        [Test(Description = "Styles should apply the specified styles to the parser.")]
        public void Styles_CustomStyles_ShouldSetStylesOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<DecimalOptionParser>(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<DecimalOptionParser>(It.Is<PropertyInfo>(p => p.Name == "Decimal"))).Returns(optionParserMock.Object);
            var setup = new DecimalOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.Decimal));

            var returnedSetup = setup.Styles(NumberStyles.AllowDecimalPoint);

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.NumberStyles = It.Is<NumberStyles>(ns => ns == NumberStyles.AllowDecimalPoint), Times.Once());
        }
    }
}