using NSubstitute;
using StoreApi.DTOs;
using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Services;

namespace StoreApi.Tests;

public class ProductServiceTests
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _service = new ProductService(
            _productRepository,
            _categoryRepository);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnProducts_WhenProductsExist()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "Gaming laptop",
                Price = 12999,
                CategoryId = 1,
                Category = new Category
                {
                    Id = 1,
                    Name = "Electronics"
                }
            }
        };

        _productRepository
            .GetAllWithCategoryAsync()
            .Returns(Task.FromResult<IEnumerable<Product>>(products));

        // Act
        var result = (await _service.GetAllAsync()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Laptop", result[0].Name);
        Assert.Equal("Electronics", result[0].CategoryName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 12999,
            CategoryId = 1,
            Category = new Category
            {
                Id = 1,
                Name = "Electronics"
            }
        };

        _productRepository
            .GetByIdWithCategoryAsync(1)
            .Returns(Task.FromResult<Product?>(product));

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal("Electronics", result.CategoryName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        _productRepository
            .GetByIdWithCategoryAsync(99)
            .Returns(Task.FromResult<Product?>(null));

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateProduct_WhenCategoryExists()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Electronics"
        };

        var dto = new CreateProductDto
        {
            Name = "Laptop",
            Description = "Gaming laptop",
            Price = 12999,
            CategoryId = 1
        };

        _categoryRepository
            .GetByIdAsync(1)
            .Returns(Task.FromResult<Category?>(category));

        _productRepository
            .AddAsync(Arg.Any<Product>())
            .Returns(call => Task.FromResult(call.Arg<Product>()));

        _productRepository
            .SaveChangesAsync()
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.Equal("Laptop", result.Name);
        Assert.Equal("Electronics", result.CategoryName);

        await _productRepository
            .Received(1)
            .AddAsync(Arg.Any<Product>());

        await _productRepository
            .Received(1)
            .SaveChangesAsync();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Laptop",
            Description = "Gaming laptop",
            Price = 12999,
            CategoryId = 99
        };

        _categoryRepository
            .GetByIdAsync(99)
            .Returns(Task.FromResult<Category?>(null));

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateAsync(dto));

        // Assert
        Assert.Equal("Category does not exist.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "Gaming Laptop",
            Description = "Updated laptop",
            Price = 14999,
            CategoryId = 1
        };

        _productRepository
            .GetByIdAsync(99)
            .Returns(Task.FromResult<Product?>(null));

        // Act
        var result = await _service.UpdateAsync(99, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 12999,
            CategoryId = 1
        };

        var category = new Category
        {
            Id = 1,
            Name = "Electronics"
        };

        var dto = new UpdateProductDto
        {
            Name = "Gaming Laptop",
            Description = "Updated laptop",
            Price = 14999,
            CategoryId = 1
        };

        _productRepository
            .GetByIdAsync(1)
            .Returns(Task.FromResult<Product?>(product));

        _categoryRepository
            .GetByIdAsync(1)
            .Returns(Task.FromResult<Category?>(category));

        _productRepository
            .SaveChangesAsync()
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        Assert.True(result);
        Assert.Equal("Gaming Laptop", product.Name);
        Assert.Equal(14999, product.Price);

        _productRepository
            .Received(1)
            .Update(product);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Laptop"
        };

        _productRepository
            .GetByIdAsync(1)
            .Returns(Task.FromResult<Product?>(product));

        _productRepository
            .SaveChangesAsync()
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);

        _productRepository
            .Received(1)
            .Delete(product);

        await _productRepository
            .Received(1)
            .SaveChangesAsync();
    }
}
