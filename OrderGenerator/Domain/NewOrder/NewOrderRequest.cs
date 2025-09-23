namespace OrderGenerator.Domain.NewOrder
{
    public class NewOrderRequest
    {
        public Guid OrderId { get; set; }
        public string Symbol { get; set; }
        public char Side { get; set; }
        public int OrderQty { get; set; }
        public decimal Price { get; set; }
    }
}
