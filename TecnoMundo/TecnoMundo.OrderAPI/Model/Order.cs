using GeekShopping.OrderAPI.Model;

namespace TecnoMundo.OrderAPI.Model
{
    public class Order
    {
        public OrderHeader OrderHeader { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }

        public Order(OrderHeader orderHeader, List<OrderDetail> orderDetail)
        {
            OrderHeader = orderHeader;
            OrderDetail = orderDetail;
        }
    }
}
