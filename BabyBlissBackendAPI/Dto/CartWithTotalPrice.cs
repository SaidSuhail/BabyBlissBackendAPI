﻿namespace BabyBlissBackendAPI.Dto
{
    public class CartWithTotalPrice
    {
        public int TotalCartPrice { get; set; }

        public List<CartViewDto> c_items { get; set; }
    }
}
