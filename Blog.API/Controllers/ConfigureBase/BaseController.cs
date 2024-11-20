using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Migrations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Controllers.ConfigureBase
{
    public abstract class BaseController : ControllerBase
    {
        private readonly IMediator _mediator;


        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<IActionResult> RequestService<TRequest, TResponse>(TRequest request, Func<Response<TResponse>, IActionResult> successResult, Func<string, IActionResult> failureResult) where TRequest : IRequest<Response<TResponse>>
        {
            var result = await _mediator.Send(request);

            if (result == null || !result.Success)
            {
                return failureResult(result?.Message ?? "Erro ao processar a requisição");
            }

            return successResult(result);
        }
    }
}
