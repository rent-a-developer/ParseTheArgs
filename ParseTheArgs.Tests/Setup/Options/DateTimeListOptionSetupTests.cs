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
    public class DateTimeListOptionSetupTests
    {
        [Test(Description = "FormatProvider should apply the specified format to the parser.")]
        public void FormatProvider_CustomFormat_ShouldSetFormatOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DateTimeListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DateTimeListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("DateTimes"))).Returns(optionParser);

            var setup = new DateTimeListOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.DateTimes));

            var returnedSetup = setup.Format("ddd dd MMM yyyy h:mm tt");

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.DateTimeFormat).To("ddd dd MMM yyyy h:mm tt").MustHaveHappened();
        }

        [Test(Description = "FormatProvider should apply the specified format provider to the parser.")]
        public void FormatProvider_CustomFormatProvider_ShouldSetFormatProviderOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DateTimeListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DateTimeListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("DateTimes"))).Returns(optionParser);
            
            var setup = new DateTimeListOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.DateTimes));

            var returnedSetup = setup.FormatProvider(new CultureInfo("en-GB"));

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.FormatProvider).To(new CultureInfo("en-GB")).MustHaveHappened();
        }

        [Test(Description = "Styles should apply the specified styles to the parser.")]
        public void Styles_CustomStyles_ShouldSetStylesOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DateTimeListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DateTimeListOptionParser>(typeof(DataTypesCommandOptions).GetProperty("DateTimes"))).Returns(optionParser);
            
            var setup = new DateTimeListOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.DateTimes));

            var returnedSetup = setup.Styles(DateTimeStyles.AdjustToUniversal);

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.DateTimeStyles).To(DateTimeStyles.AdjustToUniversal).MustHaveHappened();
        }
    }
}