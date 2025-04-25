namespace BabyBlissBackendAPI.Dto
{
    public class OrderViewDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderString { get; set; }

        public string TransactionId { get; set; }
        public List<OrderItemDto> Items { get; set; }

    }
}
