﻿using System;
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
    public class TimeListSpanOptionSetupTests
    {
        [Test(Description = "FormatProvider should apply the specified format to the parser.")]
        public void FormatProvider_CustomFormat_ShouldSetFormatOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<TimeSpanListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<TimeSpanListOptionParser>(It.Is<PropertyInfo>(p => p.Name == "TimeSpans"))).Returns(optionParserMock.Object);
            var setup = new TimeSpanListOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.TimeSpans));

            var returnedSetup = setup.Format(@"h\:mm");

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.TimeSpanFormat = It.Is<String>(s => s == @"h\:mm"), Times.Once());
        }

        [Test(Description = "FormatProvider should apply the specified format provider to the parser.")]
        public void FormatProvider_CustomFormatProvider_ShouldSetFormatProviderOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<TimeSpanListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<TimeSpanListOptionParser>(It.Is<PropertyInfo>(p => p.Name == "TimeSpans"))).Returns(optionParserMock.Object);
            var setup = new TimeSpanListOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.TimeSpans));

            var returnedSetup = setup.FormatProvider(new CultureInfo("en-GB"));

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.FormatProvider = It.Is<CultureInfo>(ci => ci.Name == "en-GB"), Times.Once());
        }

        [Test(Description = "Styles should apply the specified styles to the parser.")]
        public void Styles_CustomStyles_ShouldSetStylesOnParser()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<DataTypesCommandOptions>>(parserMock.Object);
            var optionParserMock = new Mock<TimeSpanListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            commandParserMock.Setup(cp => cp.GetOrCreateOptionParser<TimeSpanListOptionParser>(It.Is<PropertyInfo>(p => p.Name == "TimeSpans"))).Returns(optionParserMock.Object);
            var setup = new TimeSpanListOptionSetup<DataTypesCommandOptions>(commandParserMock.Object, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.TimeSpans));

            var returnedSetup = setup.Styles(TimeSpanStyles.AssumeNegative);

            returnedSetup.Should().Be(setup);

            optionParserMock.VerifySet(op => op.TimeSpanStyles = It.Is<TimeSpanStyles>(tss => tss == TimeSpanStyles.AssumeNegative), Times.Once());
        }
    }
}