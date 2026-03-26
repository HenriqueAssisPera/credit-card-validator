using CreditCardValidator.Features.RegisterCard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CreditCardValidator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterCardCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsValid)
            return UnprocessableEntity(result);

        return Created(string.Empty, result);
    }
}