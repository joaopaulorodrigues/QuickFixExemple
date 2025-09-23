using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderGenerator.Application.UseCases.Interfaces.Interfaces;
using OrderGenerator.Domain;
using OrderGenerator.Domain.NewOrder;

namespace OrderGenerator.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly INewOrderUseCase _newOrderUseCase;
    private readonly OrdersInfo _ordersInfo;
    private readonly IValidator<NewOrderRequest> _newOrderRequestValidator;

    public OrderController(INewOrderUseCase newOrderUseCase, OrdersInfo ordersInfo, IValidator<NewOrderRequest> newOrderRequestValidator)
    {
        _newOrderUseCase = newOrderUseCase;
        _ordersInfo = ordersInfo;
        _newOrderRequestValidator =  newOrderRequestValidator;
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid  id)
    {
        if (_ordersInfo.Status.TryGetValue(id, out OrderStatus orderStatus))
        {
            return Ok(orderStatus.ToString());
        }
        return NoContent();
    }
    
    [HttpPost]
    public IActionResult Post(NewOrderRequest  request)
    {
        var validationResult = _newOrderRequestValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(FormatValidationErrors(validationResult));
        }
        
        request.OrderId = Guid.NewGuid();
        if (_newOrderUseCase.Create(request))
        {
            return Ok(request.OrderId);
        }
        return BadRequest();
    }
    private object FormatValidationErrors(FluentValidation.Results.ValidationResult validationResult)
    {

        var detailedErrors = validationResult.Errors
            .Select(e => new
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage,
                Code = e.ErrorCode,
                Severity = e.Severity.ToString()
            })
            .ToList();

        // For teaching purposes, return both formats to show the difference
        return new
        {
            Title = "Validation Failed",
            Status = 400,
            //SimpleErrors = simpleErrors,
            DetailedErrors = detailedErrors
        };
    }
}