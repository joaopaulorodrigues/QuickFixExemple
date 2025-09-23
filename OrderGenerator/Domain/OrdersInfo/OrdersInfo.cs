namespace OrderGenerator.Domain;

public class OrdersInfo 
{
    public IDictionary<Guid, OrderStatus> Status { get; set; } =  new Dictionary<Guid, OrderStatus>();
}
