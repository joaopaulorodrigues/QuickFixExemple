using OrderGenerator.Application.UseCases.Interfaces.Interfaces;
using OrderGenerator.Domain.NewOrder;
using OrderGenerator.Infra.Interfaces;
using QuickFix;
using QuickFix.Fields;
using Side = QuickFix.Fields.Side;
using Symbol = QuickFix.Fields.Symbol;

namespace OrderGenerator.Application.UseCases.Interfaces;

public class NewOrderUseCase(IInitiatorServices initiatorService) : INewOrderUseCase
{
    public bool Create(NewOrderRequest request)
    {
        QuickFix.FIX44.NewOrderSingle m = QueryNewOrderSingle44(request);
        return initiatorService.SendMessage(m);;
    }
    private QuickFix.FIX44.NewOrderSingle QueryNewOrderSingle44(NewOrderRequest request)
    {
        OrdType ordType = new OrdType(OrdType.MARKET);

        QuickFix.FIX44.NewOrderSingle newOrderSingle = new QuickFix.FIX44.NewOrderSingle(
            new ClOrdID("qw"),
            new Symbol(request.Symbol),
            new Side(request.Side),
            new TransactTime(DateTime.Now),
            ordType);

        newOrderSingle.Set(new HandlInst('1'));
        newOrderSingle.Set(new OrderQty(request.OrderQty));
        newOrderSingle.Set(new TimeInForce(TimeInForce.IMMEDIATE_OR_CANCEL));
        newOrderSingle.Set(new Price(request.Price));

        return newOrderSingle;
    }

}