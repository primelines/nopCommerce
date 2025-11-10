using FluentAssertions;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using NUnit.Framework;

namespace Nop.Tests.Nop.Services.Tests.Catalog;

[TestFixture]
public class ProductServiceTests : ServiceTest
{
    #region Fields

    private IProductService _productService;

    #endregion

    #region SetUp

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _productService = GetService<IProductService>();

        var product = await _productService.GetProductByIdAsync(1);
        product.ManageInventoryMethod = ManageInventoryMethod.ManageStock;
        product.UseMultipleWarehouses = true;

        await _productService.UpdateProductAsync(product);

        await _productService.InsertProductWarehouseInventoryAsync(new ProductWarehouseInventory
        {
            ProductId = product.Id,
            WarehouseId = 1,
            StockQuantity = 8,
            ReservedQuantity = 5
        });

        await _productService.InsertProductWarehouseInventoryAsync(new ProductWarehouseInventory
        {
            ProductId = product.Id,
            WarehouseId = 2,
            StockQuantity = 5
        });
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        var product = await _productService.GetProductByIdAsync(1);
        foreach (var productWarehouseInventory in await _productService.GetAllProductWarehouseInventoryRecordsAsync(1))
            await _productService.DeleteProductWarehouseInventoryAsync(productWarehouseInventory);

        product.ManageInventoryMethod = ManageInventoryMethod.DontManageStock;
        product.UseMultipleWarehouses = false;

        await _productService.UpdateProductAsync(product);
    }

    #endregion

    #region Tests

    [Test]
    public void CanParseRequiredProductIds()
    {
        var product = new Product
        {
            RequiredProductIds = "1, 4,7 ,a,"
        };

        var ids = _productService.ParseRequiredProductIds(product);
        ids.Length.Should().Be(3);
        ids[0].Should().Be(1);
        ids[1].Should().Be(4);
        ids[2].Should().Be(7);
    }

    [Test]
    public void ShouldBeAvailableWhenStartDateIsNotSet()
    {
        var product = new Product
        {
            AvailableStartDateTimeUtc = null
        };

        _productService.ProductIsAvailable(product, new DateTime(2010, 01, 03)).Should().BeTrue();
    }

    [Test]
    public void ShouldBeAvailableWhenStartDateIsLessThanSomeDate()
    {
        var product = new Product
        {
            AvailableStartDateTimeUtc = new DateTime(2010, 01, 02)
        };

        _productService.ProductIsAvailable(product, new DateTime(2010, 01, 03)).Should().BeTrue();
    }

    [Test]
    public void ShouldNotBeAvailableWhenStartDateIsGreaterThanSomeDate()
    {
        var product = new Product
        {
            AvailableStartDateTimeUtc = new DateTime(2010, 01, 02)
        };

        _productService.ProductIsAvailable(product, new DateTime(2010, 01, 01)).Should().BeFalse();
    }

    [Test]
    public void ShouldBeAvailableWhenEndDateIsNotSet()
    {
        var product = new Product
        {
            AvailableEndDateTimeUtc = null
        };

        _productService.ProductIsAvailable(product, new DateTime(2010, 01, 03)).Should().BeTrue();
    }

    [Test]
    public void ShouldBeAvailableWhenEndDateIsGreaterThanSomeDate()
    {
        var product = new Product
        {
            AvailableEndDateTimeUtc = new DateTime(2010, 01, 02)
        };

        _productService.ProductIsAvailable(product, new DateTime(2010, 01, 01)).Should().BeTrue();
    }

    [Test]
    public void ShouldNotBeAvailableWhenEndDateIsLessThanSomeDate()
    {
        var product = new Product
        {
            AvailableEndDateTimeUtc = new DateTime(2010, 01, 02)
        };

        _productService.ProductIsAvailable(product, new DateTime(2010, 01, 03)).Should().BeFalse();
    }

    [Test]
    public void ShouldBeAvailableWhenCurrentDateIsInRange()
    {
        var product = new Product
        {
            AvailableStartDateTimeUtc = DateTime.UtcNow.AddDays(-1),
            AvailableEndDateTimeUtc = DateTime.UtcNow.AddDays(1)
        };

        _productService.ProductIsAvailable(product).Should().BeTrue();
    }

    [Test]
    public void ShouldNotBeAvailableWhenCurrentDateIsNotInRange()
    {
        var product = new Product
        {
            AvailableStartDateTimeUtc = DateTime.UtcNow.AddDays(-2),
            AvailableEndDateTimeUtc = DateTime.UtcNow.AddDays(-1)
        };

        _productService.ProductIsAvailable(product).Should().BeFalse();
    }

    [Test]
    public void CanParseAllowedQuantities()
    {
        var product = new Product
        {
            AllowedQuantities = "1, 5,4,10,sdf"
        };

        var result = _productService.ParseAllowedQuantities(product);
        result.Length.Should().Be(4);
        result[0].Should().Be(1);
        result[1].Should().Be(5);
        result[2].Should().Be(4);
        result[3].Should().Be(10);
    }

    [Test]
    public async Task CanCalculateTotalQuantityWhenWeDoNotUseMultipleWarehouses()
    {
        var result = await _productService.GetTotalStockQuantityAsync(new Product { StockQuantity = 6, ManageInventoryMethod = ManageInventoryMethod.ManageStock });
        result.Should().Be(6);
    }

    [Test]
    public async Task PublicVoidCanCalculateTotalQuantityWhenWeDoUseMultipleWarehousesWithReserved()
    {
        var result = await _productService.GetTotalStockQuantityAsync(await _productService.GetProductByIdAsync(1));
        result.Should().Be(8);
    }

    [Test]
    public async Task CanCalculateTotalQuantityWhenWeDoUseMultipleWarehousesWithoutReserved()
    {
        var result = await _productService.GetTotalStockQuantityAsync(await _productService.GetProductByIdAsync(1), false);
        result.Should().Be(13);
    }

    [Test]
    public async Task CanCalculateTotalQuantityWhenWeDoUseMultipleWarehousesWithWarehouseSpecified()
    {
        var result = await _productService.GetTotalStockQuantityAsync(await _productService.GetProductByIdAsync(1), true, 1);
        result.Should().Be(3);
    }

    #endregion
}