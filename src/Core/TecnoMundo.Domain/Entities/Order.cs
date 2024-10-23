namespace TecnoMundo.Domain.Entities
{
    public class Order
    {
        public OrderHeader OrderHeader { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }

        public Order() { }

        public Order(OrderHeader orderHeader, List<OrderDetail> orderDetail)
        {
            OrderHeader = orderHeader;
            OrderDetail = orderDetail;
        }
    }
}
