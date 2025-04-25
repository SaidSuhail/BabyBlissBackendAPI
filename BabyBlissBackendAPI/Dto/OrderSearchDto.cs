namespace BabyBlissBackendAPI.Dto
{
    public class OrderSearchDto
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDateFirst { get; set; }
        public DateTime OrderDateEnd { get; set; }
        public int userId { get; set; }


    }
}
