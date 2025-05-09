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
        public decimal TotalPrice { get; set; }
        //public decimal TotalAmount { get; internal set; }
        public string CustomerName { get; set; } // ✅ Add this line

    }
}
