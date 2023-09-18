using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CommonApi.Common.Filters;

/// <summary>
/// 请求验证过滤器
/// </summary>
public sealed class ValidationFilter<T> : ActionFilterAttribute where T : class
{
    /// <inheritdoc/>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var validationContext = context.ActionArguments.FirstOrDefault();
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is null)
        {
            throw new ArgumentNullException($"请为{typeof(T).FullName}编写验证规则");
        }

        //确保泛型类型和请求类型匹配
        if (validationContext.Value is T request)
        {
            var results = validator.Validate(request);
            if (!results.IsValid)
            {
                var errors = results.Errors.Select(error => error.ErrorMessage);
                var message = string.Join(';', errors);
                context.Result = new BadRequestObjectResult(message);
            }
        }
        else
        {
            throw new ArgumentNullException($"{typeof(T).FullName}类型和请求类型不符");
        }
    }

    /// <inheritdoc/>
    public override void OnActionExecuted(ActionExecutedContext context)
    { }
}
