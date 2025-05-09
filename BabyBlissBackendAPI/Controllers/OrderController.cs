using BabyBlissBackendAPI.Services.OrdersServices;
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

        [Authorize(Roles = "User")]
        [HttpPost("razor-order-create")]
        public async Task<IActionResult> RazorOrderCreate([FromQuery] long price)
        {
            try
            {
                if (price <= 0)
                {
                    return BadRequest(new ApiResponse<string>(false, "Enter a valid amount.", null, null));
                }

                // Call service to create Razorpay order
                var orderId = await _orderService.RazorPayOrderCreate(price);

                // Return the order ID in the response
                return Ok(new ApiResponse<string>(true, "Order created", orderId, null));
            }
            catch (Exception ex)
            {
                // Simplify error handling and return the top-level error message
                return BadRequest(new ApiResponse<string>(false, "Error creating order", null, ex.Message));
            }
        }

        [Authorize]
        [HttpPost("razor-payment-verify")]
        public async Task<IActionResult> RazorPaymentVerify([FromBody] PaymentDto razorpay)
        {
            try
            {
                // Validate payment details
                if (razorpay == null ||
                    string.IsNullOrEmpty(razorpay.razorpay_payment_id) ||
                    string.IsNullOrEmpty(razorpay.razorpay_order_id) ||
                    string.IsNullOrEmpty(razorpay.razorpay_signature))
                {
                    return BadRequest(new ApiResponse<string>(false, "Invalid Razorpay payment details", null, null));
                }

                // Verify payment via service
                var res = await _orderService.RazorPayment(razorpay);
                if (!res)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error in payment verification", "", "Check payment details"));
                }

                return Ok(new ApiResponse<string>(true, "Payment verified", "Success", null));
            }
            catch (Exception ex)
            {
                // Simplify error handling and return the top-level error message
                return BadRequest(new ApiResponse<string>(false, "Error verifying payment", null, ex.Message));
            }
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
        [HttpPost("Place-order-all")]
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

        [HttpGet("paginated-orders")]
        public async Task<ActionResult<PagedResponseDTO<OrderViewDto>>> GetPaginatedOrders(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Call the service to get the paginated orders
                var result = await _orderService.GetPaginatedOrders(pageNumber, pageSize);

                // Return a successful response with the result
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Return a 500 internal server error if something goes wrong
                return StatusCode(500, ex.Message);
            }
        }




    }
}
