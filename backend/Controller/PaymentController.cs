using backend.Dtos.Payment;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("createPayment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateRequest request)
        {
            var result = await _paymentService.CreatePayment(request);
            return Ok(result.PayUrl);
        }


    }
}
