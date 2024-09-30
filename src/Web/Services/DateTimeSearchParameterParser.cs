using Application.DataContracts;

namespace Web.Services;

public class DateTimeSearchParameterParser
{
    private Dictionary<string, DateTimeSearchOperator> operators = new()
    {
        { "eq", DateTimeSearchOperator.Equal },
        { "ne", DateTimeSearchOperator.NotEqual },
        { "lt", DateTimeSearchOperator.LessThan },
        { "gt", DateTimeSearchOperator.GreaterThan },
        { "ge", DateTimeSearchOperator.GreaterOrEqual },
        { "le", DateTimeSearchOperator.LessOrEqual },
    };

    // only supports following format
    // xxYYYY-MM-DD
    private const int RequiredLength = 12;

    public DateTimeSearchParameter? TryParse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (value.Length < RequiredLength)
            return null;

        var searchOperatorAlias = value[..2];

        if (!operators.TryGetValue(searchOperatorAlias, out var searchOperator))
            return null;

        var dateString = value[2..];

        if (!DateTime.TryParse(dateString, out var date))
            return null;

        var searchParameter = new DateTimeSearchParameter
        {
            Date = date,
            Operator = searchOperator,
        };

        return searchParameter;
    }
}
