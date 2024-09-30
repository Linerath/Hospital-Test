namespace Application.DataContracts;

public class DateTimeSearchParameter
{
    public DateTime Date { get; set; }
    public DateTimeSearchOperator Operator { get; set; }
}

public enum DateTimeSearchOperator
{
    Equal,
    NotEqual,
    LessThan,
    GreaterThan,
    GreaterOrEqual,
    LessOrEqual,
    //StartsAfter,
    //EndsBefore,
}