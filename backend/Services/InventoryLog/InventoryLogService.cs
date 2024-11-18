using System;
using AutoMapper;
using backend.Dto.Common;
using backend.Dtos.InventoryLog;
using backend.Entity;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class InventoryLogService : IInventoryLogService
{
    private readonly IRepository<InventoryLog> _repository;
    private readonly IMapper _mapper;


    public InventoryLogService(IRepository<InventoryLog> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PageResult<InventoryLogDto>> GetPaging(PageRequest request)
    {
        var query = _repository.AsQueryable();
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => x.CreatedAt.ToString().Contains(request.Search));
        }
        var total = await query.CountAsync();
        if (request.PageSize != null && request.PageNumber != null)
        {
            query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                        .Take(request.PageSize.Value);
        }
        var data = await query.ToListAsync();
        var dataDto = _mapper.Map<List<InventoryLogDto>>(data);
        dataDto.ForEach(x =>
        {
            x.Message = $"Cập nhật số lượng {x.ProductName} - {x.SizeName} từ {x.OldInventory} thành {x.Inventory} bởi {x.ByName}";
        });
        var pageResult = new PageResult<InventoryLogDto>
        {
            TotalRecords = total,
            PageNumber = request.PageNumber ?? 1,
            PageSize = request.PageSize ?? total,
            Items = dataDto
        };
        return pageResult;

    }

}
