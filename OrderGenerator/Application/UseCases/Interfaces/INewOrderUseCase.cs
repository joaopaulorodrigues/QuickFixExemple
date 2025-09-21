using OrderGenerator.Domain.NewOrder;

namespace OrderGenerator.Application.UseCases.Interfaces.Interfaces;

public interface INewOrderUseCase
{
    public bool Create(NewOrderRequest request);
}