using backend.Dtos.Category;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAll();
                return Ok(categories);
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
                var category = await _categoryService.Get(id);
                return Ok(category);
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
        public async Task<IActionResult> Save(CategoryCreateUpdateDto category)
        {
            try
            {
                await _categoryService.Save(category);
                return Ok();
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
                await _categoryService.Delete(id);
                return Ok();
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
}
