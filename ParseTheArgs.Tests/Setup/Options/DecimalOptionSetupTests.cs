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
    public class DecimalOptionSetupTests
    {
        [Test(Description = "FormatProvider should apply the specified format provider to the parser.")]
        public void FormatProvider_CustomFormatProvider_ShouldSetFormatProviderOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DecimalOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DecimalOptionParser>(typeof(DataTypesCommandOptions).GetProperty("Decimal"))).Returns(optionParser);
            var setup = new DecimalOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.Decimal));

            var returnedSetup = setup.FormatProvider(new CultureInfo("en-GB"));

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.FormatProvider).To(new CultureInfo("en-GB")).MustHaveHappened();
        }

        [Test(Description = "Styles should apply the specified styles to the parser.")]
        public void Styles_CustomStyles_ShouldSetStylesOnParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DecimalOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal")));

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DecimalOptionParser>(typeof(DataTypesCommandOptions).GetProperty("Decimal"))).Returns(optionParser);
            var setup = new DecimalOptionSetup<DataTypesCommandOptions>(commandParser, (Expression<Func<DataTypesCommandOptions, Object>>)(a => a.Decimal));

            var returnedSetup = setup.Styles(NumberStyles.AllowDecimalPoint);

            returnedSetup.Should().Be(setup);

            A.CallToSet(() => optionParser.NumberStyles).To(NumberStyles.AllowDecimalPoint).MustHaveHappened();
        }
    }
}