using OrderGenerator.Application.UseCases.Interfaces.Interfaces;
using OrderGenerator.Domain.NewOrder;
using OrderGenerator.Infra.Interfaces;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;
using Side = QuickFix.Fields.Side;
using Symbol = QuickFix.Fields.Symbol;

namespace OrderGenerator.Application.UseCases;

public class NewOrderUseCase(IInitiatorServices initiatorService) : INewOrderUseCase
{
    public bool Create(NewOrderRequest request)
    {
        NewOrderSingle m = QueryNewOrderSingle44(request);
        return initiatorService.SendMessage(m);
    }
    private NewOrderSingle QueryNewOrderSingle44(NewOrderRequest request)
    {
        OrdType ordType = new OrdType(OrdType.MARKET);

        NewOrderSingle newOrderSingle = new NewOrderSingle(
            new ClOrdID("d"),
            new Symbol(request.Symbol),
            new Side(request.Side),
            new TransactTime(DateTime.Now),
            ordType);

        newOrderSingle.Set(new OrderQty(request.OrderQty));
        newOrderSingle.Set(new Price(request.Price));

        return newOrderSingle;
    }

}