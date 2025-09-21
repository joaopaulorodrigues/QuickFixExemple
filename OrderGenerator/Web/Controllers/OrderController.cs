using Microsoft.AspNetCore.Mvc;
using OrderGenerator.Application.UseCases.Interfaces.Interfaces;
using OrderGenerator.Domain.NewOrder;

namespace OrderGenerator.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly INewOrderUseCase _newOrderUseCase;
    public OrderController(INewOrderUseCase newOrderUseCase)
    {
        _newOrderUseCase = newOrderUseCase;
    }

    [HttpPost]
    public IActionResult Post(NewOrderRequest  request)
    {
        if (_newOrderUseCase.Create(request))
        {
            return Ok();
        }
        return BadRequest();
    }
}