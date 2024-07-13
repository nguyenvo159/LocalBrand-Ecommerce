using backend.Dto.Cart;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] CartItemCreateDto cartItemCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cartDto = await _cartService.AddToCart(cartItemCreateDto);
            return Ok(cartDto);
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
    public async Task<IActionResult> UpdateCartItem([FromBody] CartItemUpdateDto cartItemUpdateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cartItemDto = await _cartService.UpdateCartItem(cartItemUpdateDto);
            return Ok(cartItemDto);
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
    public async Task<ActionResult<CartDto>> GetById(Guid id)
    {
        try
        {
            return Ok(await _cartService.GetById(id));
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

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<CartDto>> GetByUserId(Guid userId)
    {
        try
        {
            return Ok(await _cartService.GetByUserId(userId));
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
    [HttpDelete("cartitem/{id}")]
    public async Task<IActionResult> RemoveCartItem(Guid id)
    {
        try
        {
            await _cartService.RemoveItem(id);
            return Ok("Cart item deleted successfully.");
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
    public async Task<IActionResult> ClearCart(Guid id)
    {
        try
        {
            await _cartService.ClearCart(id);
            return Ok("Cart cleared successfully.");
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
