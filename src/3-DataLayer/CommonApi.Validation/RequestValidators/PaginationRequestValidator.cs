using CommonApi.DTO.Requests;
using FluentValidation;

namespace CommonApi.Validation.RequestValidators;

/// <summary>
/// 分页请求验证
/// </summary>
/// <typeparam name="T"> </typeparam>
public class PaginationRequestValidator<T> : AbstractValidator<T> where T : PaginationRequest
{
    protected PaginationRequestValidator()
    {
        RuleFor(pagination => pagination.PageSize).GreaterThanOrEqualTo(1);
        RuleFor(pagination => pagination.Current).GreaterThanOrEqualTo(1);
    }
}
