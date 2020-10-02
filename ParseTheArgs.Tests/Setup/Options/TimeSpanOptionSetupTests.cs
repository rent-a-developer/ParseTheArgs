using System;
using System.Globalization;
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
    public class TimeSpanOptionSetupTests
    {
        [Test(Description = "FormatProvider should apply the specified format to the parser.")]
        public void FormatProvider_CustomFormat_ShouldSetFormatOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpan"), "timeSpan")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanOptionParser>(typeof(DataTypesCommandOptions).GetProperty("TimeSpan"))).Returns(optionParser);
            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.TimeSpan));

            var returnedSetup = setup.Format(@"h\:mm");

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.TimeSpanFormat).To(@"h\:mm").MustHaveHappened();
        }

        [Test(Description = "FormatProvider should apply the specified format provider to the parser.")]
        public void FormatProvider_CustomFormatProvider_ShouldSetFormatProviderOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpan"), "timeSpan")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanOptionParser>(typeof(DataTypesCommandOptions).GetProperty("TimeSpan"))).Returns(optionParser);
            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.TimeSpan));

            var returnedSetup = setup.FormatProvider(new CultureInfo("en-GB"));

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.FormatProvider).To(new CultureInfo("en-GB")).MustHaveHappened();
        }

        [Test(Description = "Styles should apply the specified styles to the parser.")]
        public void Styles_CustomStyles_ShouldSetStylesOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpan"), "timeSpan")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanOptionParser>(typeof(DataTypesCommandOptions).GetProperty("TimeSpan"))).Returns(optionParser);
            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.TimeSpan));

            var returnedSetup = setup.Styles(TimeSpanStyles.AssumeNegative);

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.TimeSpanStyles).To(TimeSpanStyles.AssumeNegative).MustHaveHappened();
        }
    }
}