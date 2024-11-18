using System;
using backend.Dto.Common;
using backend.Dtos.InventoryLog;

namespace backend.Services;

public interface IInventoryLogService
{
    Task<PageResult<InventoryLogDto>> GetPaging(PageRequest request);

}
