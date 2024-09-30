using Application.DataContracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Services;

namespace Web.Binders;

public class PatientBirthdateSearchParameterBinder : IModelBinder
{
    private readonly DateTimeSearchParameterParser parser;

    public PatientBirthdateSearchParameterBinder(DateTimeSearchParameterParser parser)
    {
        this.parser = parser;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var dates = bindingContext.ValueProvider.GetValue("date");

        if (dates.Length == 0)
        {
            bindingContext.Result = ModelBindingResult.Success(Enumerable.Empty<DateTimeSearchParameter>());
            return Task.CompletedTask;
        }

        var searchParams = new List<DateTimeSearchParameter>(dates.Length);
        foreach (var date in dates)
        {
            var searchParam = parser.TryParse(date);

            if (searchParam is not null)
                searchParams.Add(searchParam);
        }

        bindingContext.Result = ModelBindingResult.Success(searchParams);

        return Task.CompletedTask;
    }
}
