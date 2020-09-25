using System;
using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class ExpressionHelperTests
    {
        [Test]
        public void TestGetPropertyFromPropertyExpression()
        {
            ExpressionHelper
                .GetPropertyFromPropertyExpression((Expression<Func<Item, Object>>) ((item) => item.PropertyA))
                .Should()
                .BeSameAs(typeof(Item).GetProperty("PropertyA"));

            ExpressionHelper
                .GetPropertyFromPropertyExpression((Expression<Func<Item, Object>>) ((item) => item.PropertyB))
                .Should()
                .BeSameAs(typeof(Item).GetProperty("PropertyB"));

            ExpressionHelper
                .GetPropertyFromPropertyExpression((Expression<Func<Item, Object>>) ((item) => item.PropertyC))
                .Should()
                .BeSameAs(typeof(Item).GetProperty("PropertyC"));
        }

        [Test]
        public void TestGetPropertyFromPropertyExpression_Exceptions()
        {
            Invoking(() => 
                ExpressionHelper
                    .GetPropertyFromPropertyExpression((Expression<Func<Item, Object>>) ((item) => item.PropertyWithoutSetter))
            )
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(@"The given property expression is invalid. It must be an expression that returns the value of a property and the property must have a public setter.
Parameter name: propertyExpression");
        }

        class Item
        {
            public String PropertyA { get; set; }
            public DateTime PropertyB { get; set; }
            public Guid PropertyC { get; set; }
            public String PropertyWithoutSetter { get;  }
        }
    }
}
