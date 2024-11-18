using System.Globalization;
using AutoMapper;
using backend.Data;
using backend.Dto.Common;
using backend.Entity;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace backend.Services;

public class DiscountService : IDiscountService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Discount> _discountRepository;
    private readonly IEmailService _emailService;

    public DiscountService(IMapper mapper, IRepository<Discount> discountRepository, IEmailService emailService)
    {
        _mapper = mapper;
        _discountRepository = discountRepository;
        _emailService = emailService;
    }


    public async Task<List<Discount>> GetAll()
    {
        var discounts = await _discountRepository.AsQueryable().Where(a => a.IsActived).ToListAsync();
        if (discounts == null)
        {
            throw new ApplicationException("No discounts found");
        }
        return discounts.ToList();
    }

    public async Task<Discount> GetByCode(string code)
    {
        var discount = await _discountRepository.FindAsync(d => d.Code == code && d.IsActived);
        if (discount == null)
        {
            throw new ApplicationException("Discount not found");
        }
        return discount;
    }

    public async Task<Discount> GetById(Guid id)
    {
        var discount = await _discountRepository.FindAsync(d => d.Id == id);
        if (discount == null)
        {
            throw new ApplicationException("Discount not found");
        }
        return discount;
    }

    public async Task<List<Discount>> Create(DiscountCreateDto discountCreateDto)
    {
        var discounts = new List<Discount>();
        for (int i = 0; i < discountCreateDto.Amount; i++)
        {
            var discount = _mapper.Map<Discount>(discountCreateDto);
            discount.Code = await GenerateCode();
            discount.IsActived = true;
            await _discountRepository.AddAsync(discount);
            discounts.Add(discount);
        }
        return discounts;
    }

    private async Task<string> GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        string code;

        do
        {
            code = new string(Enumerable.Repeat(chars, 16)
                                         .Select(s => s[random.Next(s.Length)]).ToArray());
        } while (await _discountRepository.FindAsync(d => d.Code == code) != null);

        return code;
    }

    public async Task Delete(Guid id)
    {
        var discount = await _discountRepository.FindAsync(d => d.Id == id);
        if (discount == null)
        {
            throw new ApplicationException("Discount not found");
        }
        discount.IsActived = false;
        await _discountRepository.UpdateAsync(discount);
    }

    public async Task<byte[]> Export()
    {
        var discounts = await _discountRepository.AsQueryable().Where(a => a.IsActived).ToListAsync();
        if (discounts == null || !discounts.Any())
        {
            throw new ApplicationException("No discounts found");
        }

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Discounts");

            // Tiêu đề
            worksheet.Cells[1, 1].Value = "Code";
            worksheet.Cells[1, 2].Value = "Cho hóa đơn";
            worksheet.Cells[1, 3].Value = "Giảm %";
            worksheet.Cells[1, 4].Value = "Giảm tối đa";
            worksheet.Cells[1, 5].Value = "Hạn sử dụng";

            // Định dạng tiêu đề
            using (var range = worksheet.Cells[1, 1, 1, 5])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            // Nội dung
            int row = 2;
            foreach (var discount in discounts)
            {
                worksheet.Cells[row, 1].Value = discount.Code;
                worksheet.Cells[row, 2].Value = discount.RequireMoney;
                worksheet.Cells[row, 3].Value = discount.DiscountPercentage;
                worksheet.Cells[row, 4].Value = discount.MaximumDiscount;
                worksheet.Cells[row, 5].Value = discount.ExpiryDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                row++;
            }

            // Tự động điều chỉnh kích thước cột
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();
        }
    }
    public async Task<List<string>> DeleteDiscountExpired()
    {
        var discounts = await _discountRepository.AsQueryable().Where(a => a.ExpiryDate < DateTime.UtcNow && a.IsActived).ToListAsync();
        if (discounts == null || !discounts.Any())
        {
            throw new ApplicationException("No discounts found");
        }
        await _discountRepository.DeleteListAsync(discounts);
        return discounts.Select(a => a.Code).ToList();

    }

    public async Task SendDiscount(DiscountSendReq request)
    {
        var discount = await GetByCode(request.Code);
        if (discount == null)
        {
            throw new ApplicationException("Discount not found");
        }
        await _emailService.SendEmailDiscount(discount, request.Email);
    }

    public Task<PageResult<Discount>> GetPaging(PageRequest request)
    {
        var query = _discountRepository.AsQueryable().Where(d => d.IsActived);
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => x.Code.Contains(request.Search));
        }
        var data = query.ToList();

        if (request.PageSize.HasValue && request.PageNumber.HasValue)
        {
            query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
        }
        data = data.OrderBy(x => x.ExpiryDate).ToList();
        var result = new PageResult<Discount>
        {
            TotalRecords = data.Count,
            Items = data,
            PageSize = request.PageSize ?? 1,
            PageNumber = request.PageNumber ?? 1
        };
        return Task.FromResult(result);
    }
}

