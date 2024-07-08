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
        catch (Exception)
        {
            return NotFound("Not found product ");
        }
    }

    [HttpGet("category/{name}")]
    public async Task<IActionResult> GetByCategory(string name)
    {
        try
        {
            var products = await _productService.GetByCategory(name);
            if (products == null)
            {
                return NotFound("Product not found by category");
            }
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpPost]
    [Authorize(Roles = "Admin, Staff")]
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
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<ActionResult> Update(Guid id, [FromBody] ProductUpdateDto productUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != productUpdateDto.Id)
        {
            return BadRequest("Product ID mismatch");
        }

        var updatedProduct = await _productService.Update(productUpdateDto);
        if (updatedProduct == null)
        {
            return NotFound();
        }
        return CreatedAtAction(nameof(Get), new { id = updatedProduct.Id }, updatedProduct);
    }
    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound("Found not product");
            }
            if (!await _productService.Delete(id))
            {
                return BadRequest("Error while delete product.");
            }
            return Ok("Delete product successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
