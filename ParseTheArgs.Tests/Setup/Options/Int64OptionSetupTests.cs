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
    public class Int64OptionSetupTests
    {
        [Test(Description = "FormatProvider should apply the specified format provider to the parser.")]
        public void FormatProvider_CustomFormatProvider_ShouldSetFormatProviderOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<Int64OptionParser>(typeof(DataTypesCommandOptions).GetProperty("Int64"))).Returns(optionParser);
            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.Int64));

            var returnedSetup = setup.FormatProvider(new CultureInfo("en-GB"));

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.FormatProvider).To(new CultureInfo("en-GB")).MustHaveHappened();
        }

        [Test(Description = "Styles should apply the specified styles to the parser.")]
        public void Styles_CustomStyles_ShouldSetStylesOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<Int64OptionParser>(typeof(DataTypesCommandOptions).GetProperty("Int64"))).Returns(optionParser);
            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.Int64));

            var returnedSetup = setup.Styles(NumberStyles.AllowDecimalPoint);

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.NumberStyles).To(NumberStyles.AllowDecimalPoint).MustHaveHappened();
        }
    }
}