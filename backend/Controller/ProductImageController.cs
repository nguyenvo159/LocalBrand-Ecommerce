using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;

[Route("api/image")]
[ApiController]
// [Authorize(Roles = "Admin")]
public class ProductImageController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly ICloudService _cloudService;

    public ProductImageController(IImageService imageService, ICloudService cloudService)
    {
        _imageService = imageService;
        _cloudService = cloudService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(List<IFormFile> files, Guid productId)
    {
        try
        {
            await _imageService.UploadImage(files, productId);
            return Ok("Added image successfully");
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }

    }
    [HttpPost("upload-no-vector")]
    public async Task<IActionResult> UploadImageNoVector(List<IFormFile> files, Guid productId)
    {
        try
        {
            await _imageService.UploadImageNoVector(files, productId);
            return Ok("Added image successfully");
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }

    }
    [HttpPost("search-by-image")]
    public async Task<IActionResult> SearchByImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded or file is empty.");
        }
        var products = await _imageService.SearchImageAsync(file);
        return Ok(products);
    }

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        try
        {
            var url = await _cloudService.UploadImageAsync(file);
            return Ok(url);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteImage(string url)
    {
        try
        {
            await _cloudService.DeleteImageAsync(url);
            return Ok("Deleted image successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
