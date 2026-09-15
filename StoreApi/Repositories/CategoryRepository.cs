using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
