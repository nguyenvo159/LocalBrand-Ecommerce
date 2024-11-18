using backend.Dto.Common;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryLogController : ControllerBase
    {
        private readonly IInventoryLogService _inventoryLogService;

        public InventoryLogController(IInventoryLogService inventoryLogService)
        {
            _inventoryLogService = inventoryLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging([FromQuery] PageRequest request)
        {
            try
            {
                var inventoryLogs = await _inventoryLogService.GetPaging(request);
                return Ok(inventoryLogs);
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
