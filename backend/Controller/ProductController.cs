using AutoMapper;
using backend.Data;
using backend.Dto.Product;
using backend.Entity;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controller;

[Route("api/product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var productDtos = await _productService.GetAll();
            if (productDtos == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(productDtos);
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
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var product = await _productService.GetById(id);
            return Ok(product);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
    }

    [HttpGet("image/{id}")]
    public async Task<IActionResult> GetImageByProductId(Guid id)
    {
        try
        {
            var images = await _productService.GetImageByProductId(id);
            return Ok(images);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
    }

    [HttpGet("category/{name}")]
    public async Task<IActionResult> GetByCategory(string name)
    {
        try
        {
            var products = await _productService.GetByCategory(name);
            return Ok(products);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
    }
    [HttpPost]
    //    [Authorize(Roles = "Admin, Staff")]
    public async Task<ActionResult<Product>> Create(ProductCreateDto productCreateDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productService.Create(productCreateDTO);

            return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut]
    //[Authorize(Roles = "Admin, Staff")]
    public async Task<ActionResult> Update([FromBody] ProductUpdateDto productUpdateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProduct = await _productService.Update(productUpdateDto);
            if (updatedProduct == null)
            {
                return BadRequest("Error while updating product");
            }
            return CreatedAtAction(nameof(Get), new { id = updatedProduct.Id }, updatedProduct);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }

    }
    [HttpDelete]
    [Route("{id}")]
    //[Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (!await _productService.Delete(id))
            {
                return BadRequest("Error while delete product.");
            }
            return Ok("Delete product successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("search/{keyword}")]
    public async Task<IActionResult> Search(string keyword)
    {
        try
        {
            var products = await _productService.Search(keyword);
            return Ok(products);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportProducts(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var (successCount, successNames, failedNames) = await _productService.Import(file);

            return Ok(new
            {
                SuccessCount = successCount,
                SuccessNames = successNames,
                FailedNames = failedNames
            });
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
