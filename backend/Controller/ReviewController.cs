using backend.Dto.Common;
using backend.Dto.Review;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;

[Route("api/review")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReviewCreateDto reviewCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var review = await _reviewService.Create(reviewCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
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
    public async Task<IActionResult> Update([FromBody] ReviewUpdateDto reviewUpdateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var review = await _reviewService.Update(reviewUpdateDto);
            return Ok(review);
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

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var review = await _reviewService.GetById(id);
            return Ok(review);

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

    [HttpGet]
    [Route("list/{productId}")]
    public async Task<IActionResult> GetByProductId([FromRoute] Guid productId)
    {
        try
        {
            var review = await _reviewService.GetByProductId(productId);
            return Ok(review);
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
    [Route("paging")]
    public async Task<IActionResult> GetPaging([FromBody] ReviewGetPagingReq request)
    {
        try
        {
            var review = await _reviewService.GetPaging(request);
            return Ok(review);
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

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _reviewService.Delete(id);
            return Ok("Delete review successfully");
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
