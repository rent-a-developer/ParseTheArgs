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
    public class TimeListSpanOptionSetupTests
    {
        [Test(Description = "FormatProvider should apply the specified format to the parser.")]
        public void FormatProvider_CustomFormat_ShouldSetFormatOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<TimeSpanListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"))).Returns(optionParser);
            var setup = new TimeSpanListOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.TimeSpans));

            var returnedSetup = setup.Format(@"h\:mm");

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.TimeSpanFormat).To(@"h\:mm").MustHaveHappened();
        }

        [Test(Description = "FormatProvider should apply the specified format provider to the parser.")]
        public void FormatProvider_CustomFormatProvider_ShouldSetFormatProviderOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<TimeSpanListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"))).Returns(optionParser);
            var setup = new TimeSpanListOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.TimeSpans));

            var returnedSetup = setup.FormatProvider(new CultureInfo("en-GB"));

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.FormatProvider).To(new CultureInfo("en-GB")).MustHaveHappened();
        }

        [Test(Description = "Styles should apply the specified styles to the parser.")]
        public void Styles_CustomStyles_ShouldSetStylesOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<TimeSpanListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"))).Returns(optionParser);
            var setup = new TimeSpanListOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.TimeSpans));

            var returnedSetup = setup.Styles(TimeSpanStyles.AssumeNegative);

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.TimeSpanStyles).To(TimeSpanStyles.AssumeNegative).MustHaveHappened();
        }
    }
}