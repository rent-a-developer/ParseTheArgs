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
        [Test(Description = "GetPropertyFromPropertyExpression should throw an exception when the given expression is not a valid property expression.")]
        public void GetPropertyFromPropertyExpression_InvalidExpression_ShouldThrowException()
        {
            Invoking(() =>
                    ExpressionHelper
                        .GetPropertyFromPropertyExpression((Expression<Func<Item, Object>>) ((item) => item.Method()))
                )
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given property expression is invalid. It must be an expression that returns the value of a property and the property must have a public setter.
Parameter name: propertyExpression");
        }

        [Test(Description = "GetPropertyFromPropertyExpression should throw an exception when the given expression is null.")]
        public void GetPropertyFromPropertyExpression_PropertyExpressionIsNull_ShouldThrowException()
        {
            Invoking(() =>
                    ExpressionHelper
                        .GetPropertyFromPropertyExpression(null)
                )
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "GetPropertyFromPropertyExpression should throw an exception when the given expression points to a property that does not have a public setter.")]
        public void GetPropertyFromPropertyExpression_PropertyWithoutPublicSetter_ShouldThrowException()
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

        [Test(Description = "GetPropertyFromPropertyExpression should return the correct property when the given expression is a valid property expression.")]
        public void GetPropertyFromPropertyExpression_ValidExpression_ShouldReturnCorrectProperty()
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

        class Item
        {
            public String PropertyA { get; set; }
            public DateTime PropertyB { get; set; }
            public Guid PropertyC { get; set; }

            public String PropertyWithoutSetter { get; }

            public String Method()
            {
                return null;
            }
        }
    }
}