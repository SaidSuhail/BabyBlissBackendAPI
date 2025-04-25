﻿using BabyBlissBackendAPI.Services.OrdersServices;
using Microsoft.AspNetCore.Mvc;
using BabyBlissBackendAPI.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using BabyBlissBackendAPI.Dto;
namespace BabyBlissBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController:ControllerBase
    {
        private readonly IOrderServices _orderService;
        public OrderController(IOrderServices orderService)
        {
            _orderService = orderService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("Manage-order-status/{oid}")]
        public async Task<IActionResult>OrderStatus(int oid)
        {
            try
            {
                var res = await _orderService.UpdateOrderStatus(oid);
                return Ok(new ApiResponse<string>(true, "Updated", res, null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        [Authorize]
        [HttpPost("Place-order-individual/{pro_id}")]
        public async Task<IActionResult>individual_probuy(int pro_id,CreateOrderDto dto)
        {
            try
            {
                if(dto == null)
                {
                    return BadRequest("Order-details-are-required");
                }
                var user_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _orderService.Indidvidual_ProductBuy(user_id, pro_id, dto);

                return Ok(new ApiResponse<string>(true, "Product purchased successfully", "done", null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(" Place-order-all")]
        [Authorize]
        public async Task<IActionResult>PlaceOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                var user_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _orderService.CrateOrder_CheckOut(user_id, createOrderDto);
                return Ok(new ApiResponse<string>(true, "successfully placed", "done", null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Get-order-Details")]
        public async Task<IActionResult>GetOrderDetails()
        {
            try
            {
                var u_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _orderService.GetOrderDetails(u_id);
                return Ok(new ApiResponse<IEnumerable<OrderViewDto>>(true, "successfully ordered", res, null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Get-order-Details-Admin")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>GetOrderDetailsAdmin()
        {
            try
            {
                var res = await _orderService.GetOrderDetailsAdmin();
                if(res.Count<0)
                {
                    return BadRequest(new ApiResponse<string>(false, "no order found", null, null));
                }
                return Ok(new ApiResponse<IEnumerable<OrderAdminViewDto>>(true, "successfully", res, null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Total Revenue")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>TotalRevenue()
        {
            try
            {
                var res = await _orderService.TotalRevenue();
                return Ok(new ApiResponse<decimal>(true, "successfully", res, null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Total-Products-Sold")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> TotalProductsPurchased()
        {
            try
            {
                var res = await _orderService.TotalProductsPurchased();
                return Ok(new ApiResponse<int>(true, "successfully", res, null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetOrderDetailsAdmin-ByuserId/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>GetOrderDetailsAdmin_byuserId(int id)
        {
            try
            {
                var orderDetails = await _orderService.GetOrderDetailsAdmin_byuserId(id);
                if(orderDetails == null)
                {
                    return NotFound("User not found");
                }
                return Ok(new ApiResponse<IEnumerable<OrderViewDto>>(true, "done", orderDetails, null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
