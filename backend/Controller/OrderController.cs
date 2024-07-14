using backend.Dto.Order;
using backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;

[Route("api/order")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var orders = await _orderService.GetAll();
            return Ok(orders);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {

            throw new Exception("Error:", ex);
        }
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetAllByUserId(Guid id)
    {
        try
        {
            var orders = await _orderService.GetAllByUserId(id);
            return Ok(orders);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {

            throw new Exception("Error:", ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var order = await _orderService.GetById(id);
            return Ok(order);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {

            throw new Exception("Error:", ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto orderCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = await _orderService.Create(orderCreateDto);
            return Ok(order);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {

            throw new Exception("Error:", ex);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] OrderUpdateDto orderUpdateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = await _orderService.Update(orderUpdateDto);
            return Ok(order);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {

            throw new Exception("Error:", ex);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _orderService.Delete(id);
            return Ok("Order deleted successfully");
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {

            throw new Exception("Error:", ex);
        }
    }

}
