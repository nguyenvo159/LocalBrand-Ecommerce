using backend.Data;
using backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace backend;

public class SizeRepository : ISizeRepository
{
    private readonly AppDbContext _context;
    public SizeRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Size> GetByNameAsync(string name)
    {
        return await _context.Sizes.FirstOrDefaultAsync(x => x.Name == name);
    }
}
