using Meetup.Api.Models;
using Meetup.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Meetup.Api.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid)
        {
            await next();
        }

        var modelStateErrors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .ToDictionary(x => x.Key, x => x.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

        var validationErrorResponse = new ValidationErrorResponse();

        foreach (var error in modelStateErrors)
        {
            validationErrorResponse.Errors.Add(
                new ValidationErrorModel
                {
                    FieldName = error.Key,
                    Messages = error.Value.ToList()
                });
        }

        context.Result = new BadRequestObjectResult(validationErrorResponse);
    }
}