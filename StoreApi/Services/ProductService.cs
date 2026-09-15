using StoreApi.DTOs;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllWithCategoryAsync();

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? string.Empty
        });
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdWithCategoryAsync(id);

        if (product is null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty
        };
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);

        if (category is null)
            throw new ArgumentException("Category does not exist.");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = category.Name
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return false;

        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);

        if (category is null)
            throw new ArgumentException("Category does not exist.");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return false;

        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }
}
