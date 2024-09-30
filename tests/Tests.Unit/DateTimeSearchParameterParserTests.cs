using Application.DataContracts;
using Shouldly;
using Web.Services;

namespace Tests.Unit;

public class DateTimeSearchParameterParserTests
{
    public static TheoryData<string, DateTimeSearchOperator, DateTime> TestData = new()
    {
        { "eq2013-01-14", DateTimeSearchOperator.Equal, new DateTime(2013, 1, 14) },
        { "ne2013-02-14", DateTimeSearchOperator.NotEqual, new DateTime(2013, 2, 14) },
        { "lt2013-03-14T10:00", DateTimeSearchOperator.LessThan, new DateTime(2013, 3, 14, 10, 0, 0) },
        { "gt2013-04-14T10:00", DateTimeSearchOperator.GreaterThan, new DateTime(2013, 4, 14, 10, 0, 0) },
        { "ge2013-05-14", DateTimeSearchOperator.GreaterOrEqual, new DateTime(2013, 5, 14) },
        { "le2013-06-14", DateTimeSearchOperator.LessOrEqual, new DateTime(2013, 6, 14) },
    };

    [Theory, MemberData(nameof(TestData))]
    public void TryParse_ShouldWork(string value, DateTimeSearchOperator expectedOperator, DateTime expectedDate)
    {
        DateTimeSearchParameterParser parser = new();

        var parameter = parser.TryParse(value);

        parameter.ShouldNotBeNull();
        parameter.Date.ShouldBe(expectedDate);
        parameter.Operator.ShouldBe(expectedOperator);
    }
}
